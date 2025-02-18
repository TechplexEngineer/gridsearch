using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using libsecondlife;
using libsecondlife.Packets;

namespace libsecondlife.TestClient
{
    public class InventoryCommand : Command
    {
        private Inventory Inventory;
        private InventoryManager Manager;

        public InventoryCommand(TestClient testClient)
        {
            Name = "i";
            Description = "Prints out inventory.";
        }

        public override string Execute(string[] args, LLUUID fromAgentID)
        {
            Manager = Client.Inventory;
            Inventory = Manager.Store;

            StringBuilder result = new StringBuilder();

            InventoryFolder rootFolder = Inventory.RootFolder;
            PrintFolder(rootFolder, result, 0);

            return result.ToString();
        }

        void PrintFolder(InventoryFolder f, StringBuilder result, int indent)
        {
            foreach (InventoryBase i in Manager.FolderContents(f.UUID, Client.Self.AgentID, true, true, InventorySortOrder.ByName, 3000))
            {
                result.AppendFormat("{0}{1} ({2})\n", new String(' ', indent * 2), i.Name, i.UUID);
                if (i is InventoryFolder)
                {
                    InventoryFolder folder = (InventoryFolder)i;
                    PrintFolder(folder, result, indent + 1);
                }
            }
        }
    }
}
