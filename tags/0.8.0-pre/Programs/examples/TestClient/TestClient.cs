using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Xml;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.Utilities;

namespace OpenMetaverse.TestClient
{
    public class TestClient : GridClient
    {
        public UUID GroupID = UUID.Zero;
        public Dictionary<UUID, GroupMember> GroupMembers;
        public Dictionary<UUID, AvatarAppearancePacket> Appearances = new Dictionary<UUID, AvatarAppearancePacket>();
        public Dictionary<string, Command> Commands = new Dictionary<string, Command>();
        public bool Running = true;
        public bool GroupCommands = false;
        public string MasterName = String.Empty;
        public UUID MasterKey = UUID.Zero;
        public bool AllowObjectMaster = false;
        public ClientManager ClientManager;
        public VoiceManager VoiceManager;
        // Shell-like inventory commands need to be aware of the 'current' inventory folder.
        public InventoryFolder CurrentDirectory = null;

        private System.Timers.Timer updateTimer;
        private UUID GroupMembersRequestID;
        public Dictionary<UUID, Group> GroupsCache = null;
        private ManualResetEvent GroupsEvent = new ManualResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        public TestClient(ClientManager manager)
        {
            ClientManager = manager;

            updateTimer = new System.Timers.Timer(500);
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateTimer_Elapsed);

            RegisterAllCommands(Assembly.GetExecutingAssembly());

            Settings.LOG_LEVEL = Helpers.LogLevel.Debug;
            Settings.LOG_RESENDS = false;
            Settings.STORE_LAND_PATCHES = true;
            Settings.ALWAYS_DECODE_OBJECTS = true;
            Settings.ALWAYS_REQUEST_OBJECTS = true;
            Settings.SEND_AGENT_UPDATES = true;
            Settings.USE_ASSET_CACHE = true;

            Network.RegisterCallback(PacketType.AgentDataUpdate, new NetworkManager.PacketCallback(AgentDataUpdateHandler));
            Network.OnLogin += new NetworkManager.LoginCallback(LoginHandler);
            Self.IM += Self_IM;
            Groups.GroupMembersReply += GroupMembersHandler;
            Inventory.OnObjectOffered += new InventoryManager.ObjectOfferedCallback(Inventory_OnInventoryObjectReceived);

            Network.RegisterCallback(PacketType.AvatarAppearance, new NetworkManager.PacketCallback(AvatarAppearanceHandler));
            Network.RegisterCallback(PacketType.AlertMessage, new NetworkManager.PacketCallback(AlertMessageHandler));

            VoiceManager = new VoiceManager(this);

            updateTimer.Start();
        }

        void Self_IM(object sender, InstantMessageEventArgs e)
        {
            bool groupIM = e.IM.GroupIM && GroupMembers != null && GroupMembers.ContainsKey(e.IM.FromAgentID) ? true : false;

            if (e.IM.FromAgentID == MasterKey || (GroupCommands && groupIM))
            {
                // Received an IM from someone that is authenticated
                Console.WriteLine("<{0} ({1})> {2}: {3} (@{4}:{5})", e.IM.GroupIM ? "GroupIM" : "IM", e.IM.Dialog, e.IM.FromAgentName, e.IM.Message, 
                    e.IM.RegionID, e.IM.Position);

                if (e.IM.Dialog == InstantMessageDialog.RequestTeleport)
                {
                    Console.WriteLine("Accepting teleport lure.");
                    Self.TeleportLureRespond(e.IM.FromAgentID, true);
                }
                else if (
                    e.IM.Dialog == InstantMessageDialog.MessageFromAgent ||
                    e.IM.Dialog == InstantMessageDialog.MessageFromObject)
                {
                    ClientManager.Instance.DoCommandAll(e.IM.Message, e.IM.FromAgentID);
                }
            }
            else
            {
                // Received an IM from someone that is not the bot's master, ignore
                Console.WriteLine("<{0} ({1})> {2} (not master): {3} (@{4}:{5})", e.IM.GroupIM ? "GroupIM" : "IM", e.IM.Dialog, e.IM.FromAgentName, e.IM.Message,
                    e.IM.RegionID, e.IM.Position);
                return;
            }
        }

        /// <summary>
        /// Initialize everything that needs to be initialized once we're logged in.
        /// </summary>
        /// <param name="login">The status of the login</param>
        /// <param name="message">Error message on failure, MOTD on success.</param>
        public void LoginHandler(LoginStatus login, string message)
        {
            if (login == LoginStatus.Success)
            {
                // Start in the inventory root folder.
                CurrentDirectory = Inventory.Store.RootFolder;
            }
        }

        public void RegisterAllCommands(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
            {
                try
                {
                    if (t.IsSubclassOf(typeof(Command)))
                    {
                        ConstructorInfo info = t.GetConstructor(new Type[] { typeof(TestClient) });
                        Command command = (Command)info.Invoke(new object[] { this });
                        RegisterCommand(command);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void RegisterCommand(Command command)
        {
            command.Client = this;
            if (!Commands.ContainsKey(command.Name.ToLower()))
            {
                Commands.Add(command.Name.ToLower(), command);
            }
        }

        public void ReloadGroupsCache()
        {
            Groups.CurrentGroups += Groups_CurrentGroups;            
            Groups.RequestCurrentGroups();
            GroupsEvent.WaitOne(10000, false);
            Groups.CurrentGroups -= Groups_CurrentGroups;
            GroupsEvent.Reset();
        }

        void Groups_CurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
            if (null == GroupsCache)
                GroupsCache = e.Groups;
            else
                lock (GroupsCache) { GroupsCache = e.Groups; }
            GroupsEvent.Set();
        }

        public UUID GroupName2UUID(String groupName)
        {
            UUID tryUUID;
            if (UUID.TryParse(groupName,out tryUUID))
                    return tryUUID;
            if (null == GroupsCache) {
                    ReloadGroupsCache();
                if (null == GroupsCache)
                    return UUID.Zero;
            }
            lock(GroupsCache) {
                if (GroupsCache.Count > 0) {
                    foreach (Group currentGroup in GroupsCache.Values)
                        if (currentGroup.Name.ToLower() == groupName.ToLower())
                            return currentGroup.ID;
                }
            }
            return UUID.Zero;
        }      

        private void updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (Command c in Commands.Values)
                if (c.Active)
                    c.Think();
        }

        private void AgentDataUpdateHandler(Packet packet, Simulator sim)
        {
            AgentDataUpdatePacket p = (AgentDataUpdatePacket)packet;
            if (p.AgentData.AgentID == sim.Client.Self.AgentID)
            {
                GroupID = p.AgentData.ActiveGroupID;

                GroupMembersRequestID = sim.Client.Groups.RequestGroupMembers(GroupID);
            }
        }

        private void GroupMembersHandler(object sender, GroupMembersReplyEventArgs e)
        {
            if (e.RequestID != GroupMembersRequestID) return;

            GroupMembers = e.Members;
        }

        private void AvatarAppearanceHandler(Packet packet, Simulator simulator)
        {
            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            lock (Appearances) Appearances[appearance.Sender.ID] = appearance;
        }

        private void AlertMessageHandler(Packet packet, Simulator simulator)
        {
            AlertMessagePacket message = (AlertMessagePacket)packet;

            Logger.Log("[AlertMessage] " + Utils.BytesToString(message.AlertData.Message), Helpers.LogLevel.Info, this);
        }
       
        private bool Inventory_OnInventoryObjectReceived(InstantMessage offer, AssetType type,
            UUID objectID, bool fromTask)
        {
            if (MasterKey != UUID.Zero)
            {
                if (offer.FromAgentID != MasterKey)
                    return false;
            }
            else if (GroupMembers != null && !GroupMembers.ContainsKey(offer.FromAgentID))
            {
                return false;
            }

            return true;
        }
    }
}
