using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace OpenMetaverse.TestClient
{
    public class LoginDetails
    {
        public string FirstName;
        public string LastName;
        public string Password;
        public string StartLocation;
        public bool GroupCommands;
        public string MasterName;
        public Guid MasterKey;
        public string URI;
    }

    public class StartPosition
    {
        public string sim;
        public int x;
        public int y;
        public int z;

        public StartPosition()
        {
            this.sim = null;
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
    }

    // WOW WHAT A HACK!
    public static class ClientManagerRef
    {
        public static ClientManager ClientManager;
    }

    public class ClientManager
    {
        public Dictionary<Guid, TestClient> Clients = new Dictionary<Guid, TestClient>();
        public Dictionary<Simulator, Dictionary<uint, Primitive>> SimPrims = new Dictionary<Simulator, Dictionary<uint, Primitive>>();

        public bool Running = true;
        public bool GetTextures = false;

        string version = "1.0.0";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accounts"></param>
        public ClientManager(List<LoginDetails> accounts, bool getTextures)
        {
            ClientManagerRef.ClientManager = this;

            GetTextures = getTextures;

            foreach (LoginDetails account in accounts)
                Login(account);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public TestClient Login(LoginDetails account)
        {
            // Check if this client is already logged in
            foreach (TestClient c in Clients.Values)
            {
                if (c.Self.FirstName == account.FirstName && c.Self.LastName == account.LastName)
                {
                    Logout(c);
                    break;
                }
            }

            TestClient client = new TestClient(this);

            // Optimize the throttle
            client.Throttle.Wind = 0;
            client.Throttle.Cloud = 0;
            client.Throttle.Land = 1000000;
            client.Throttle.Task = 1000000;

            client.GroupCommands = account.GroupCommands;
			client.MasterName = account.MasterName;
            client.MasterKey = account.MasterKey;
            client.AllowObjectMaster = client.MasterKey != Guid.Empty; // Require Guid for object master.

            LoginParams loginParams = client.Network.DefaultLoginParams(
                    account.FirstName, account.LastName, account.Password, "TestClient", version);

            if (!String.IsNullOrEmpty(account.StartLocation))
                loginParams.Start = account.StartLocation;

            if (!String.IsNullOrEmpty(account.URI))
                loginParams.URI = account.URI;
            
            if (client.Network.Login(loginParams))
            {
                Clients[client.Self.AgentID] = client;

                if (client.MasterKey == Guid.Empty)
                {
                    Guid query = Guid.NewGuid();
                    DirectoryManager.DirPeopleReplyCallback peopleDirCallback =
                        delegate(Guid queryID, List<DirectoryManager.AgentSearchData> matchedPeople)
                        {
                            if (queryID == query)
                            {
                                if (matchedPeople.Count != 1)
                                {
                                    Logger.Log("Unable to resolve master key from " + client.MasterName, Helpers.LogLevel.Warning);
                                }
                                else
                                {
                                    client.MasterKey = matchedPeople[0].AgentID;
                                    Logger.Log("Master key resolved to " + client.MasterKey, Helpers.LogLevel.Info);
                                }
                            }
                        };

                    client.Directory.OnDirPeopleReply += peopleDirCallback;
                    client.Directory.StartPeopleSearch(DirectoryManager.DirFindFlags.People, client.MasterName, 0, query);
                }

                Logger.Log("Logged in " + client.ToString(), Helpers.LogLevel.Info);
            }
            else
            {
                Logger.Log("Failed to login " + account.FirstName + " " + account.LastName + ": " +
                    client.Network.LoginMessage, Helpers.LogLevel.Warning);
            }

            return client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public TestClient Login(string[] args)
        {
            LoginDetails account = new LoginDetails();
            account.FirstName = args[0];
            account.LastName = args[1];
            account.Password = args[2];

            if (args.Length > 3)
                account.StartLocation = NetworkManager.StartLocation(args[3], 128, 128, 40);
            if (args.Length > 4)
                account.URI = args[4];

            return Login(account);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Type quit to exit.  Type help for a command list.");

            while (Running)
            {
                PrintPrompt();
                string input = Console.ReadLine();
                DoCommandAll(input, Guid.Empty);
            }

            foreach (GridClient client in Clients.Values)
            {
                if (client.Network.Connected)
                    client.Network.Logout();
            }
        }

        private void PrintPrompt()
        {
            int online = 0;

            foreach (GridClient client in Clients.Values)
            {
                if (client.Network.Connected) online++;
            }

            Console.Write(online + " avatars online> ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="fromAgentID"></param>
        /// <param name="imSessionID"></param>
        public void DoCommandAll(string cmd, Guid fromAgentID)
        {
            string[] tokens = cmd.Trim().Split(new char[] { ' ', '\t' });
            if (tokens.Length == 0)
                return;
            
            string firstToken = tokens[0].ToLower();
            if (String.IsNullOrEmpty(firstToken))
                return;

            string[] args = new string[tokens.Length - 1];
            if (args.Length > 0)
                Array.Copy(tokens, 1, args, 0, args.Length);

            if (firstToken == "login")
            {
                Login(args);
            }
            else if (firstToken == "quit")
            {
                Quit();
                Logger.Log("All clients logged out and program finished running.", Helpers.LogLevel.Info);
            }
            else if (firstToken == "help")
            {
                if (Clients.Count > 0)
                {
                    foreach (TestClient client in Clients.Values)
                    {
                        Console.WriteLine(client.Commands["help"].Execute(args, Guid.Empty));
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("You must login at least one bot to use the help command");
                }
            }
            else if (firstToken == "script")
            {
                // No reason to pass this to all bots, and we also want to allow it when there are no bots
                ScriptCommand command = new ScriptCommand(null);
                Logger.Log(command.Execute(args, Guid.Empty), Helpers.LogLevel.Info);
            }
            else
            {
                // Make an immutable copy of the Clients dictionary to safely iterate over
                Dictionary<Guid, TestClient> clientsCopy = new Dictionary<Guid, TestClient>(Clients);

                int completed = 0;

                foreach (TestClient client in clientsCopy.Values)
                {
                    ThreadPool.QueueUserWorkItem((WaitCallback)
                        delegate(object state)
                        {
                            TestClient testClient = (TestClient)state;
                            if (testClient.Commands.ContainsKey(firstToken))
                                Logger.Log(testClient.Commands[firstToken].Execute(args, fromAgentID),
                                    Helpers.LogLevel.Info, testClient);
                            else
                                Logger.Log("Unknown command " + firstToken, Helpers.LogLevel.Warning);

                            ++completed;
                        },
                        client);
                }

                while (completed < clientsCopy.Count)
                    Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Logout(TestClient client)
        {
            Clients.Remove(client.Self.AgentID);
            client.Network.Logout();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Quit()
        {
            Running = false;
            // TODO: It would be really nice if we could figure out a way to abort the ReadLine here in so that Run() will exit.
        }
    }
}
