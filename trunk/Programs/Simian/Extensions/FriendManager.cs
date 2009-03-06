using System;
using System.Collections.Generic;
using ExtensionLoader;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace Simian.Extensions
{
    public class FriendManager : IExtension<Simian>
    {
        Simian server;

        public FriendManager()
        {
        }

        public void Start(Simian server)
        {
            this.server = server;

            server.UDP.RegisterPacketCallback(PacketType.ImprovedInstantMessage, new PacketCallback(ImprovedInstantMessageHandler));
        }

        public void Stop()
        {
        }

        void ImprovedInstantMessageHandler(Packet packet, Agent agent)
        {
            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)packet;
            InstantMessageDialog dialog = (InstantMessageDialog)im.MessageBlock.Dialog;

            if (dialog == InstantMessageDialog.FriendshipOffered || dialog == InstantMessageDialog.FriendshipAccepted || dialog == InstantMessageDialog.FriendshipDeclined)
            {
                // HACK: Only works for agents currently online
                Agent recipient;
                if (server.Scene.TryGetAgent(im.MessageBlock.ToAgentID, out recipient))
                {
                    ImprovedInstantMessagePacket sendIM = new ImprovedInstantMessagePacket();
                    sendIM.MessageBlock.RegionID = server.Scene.RegionID;
                    sendIM.MessageBlock.ParentEstateID = 1;
                    sendIM.MessageBlock.FromGroup = false;
                    sendIM.MessageBlock.FromAgentName = Utils.StringToBytes(agent.FullName);
                    sendIM.MessageBlock.ToAgentID = im.MessageBlock.ToAgentID;
                    sendIM.MessageBlock.Dialog = im.MessageBlock.Dialog;
                    sendIM.MessageBlock.Offline = (byte)InstantMessageOnline.Online;
                    sendIM.MessageBlock.ID = agent.ID;
                    sendIM.MessageBlock.Message = im.MessageBlock.Message;
                    sendIM.MessageBlock.BinaryBucket = Utils.EmptyBytes;
                    sendIM.MessageBlock.Timestamp = 0;
                    sendIM.MessageBlock.Position = agent.Avatar.GetSimulatorPosition();

                    sendIM.AgentData.AgentID = agent.ID;

                    server.UDP.SendPacket(recipient.ID, sendIM, PacketCategory.Transaction);

                    if (dialog == InstantMessageDialog.FriendshipAccepted)
                    {
                        bool receiverOnline = server.Scene.ContainsObject(agent.ID);
                        bool senderOnline = server.Scene.ContainsObject(recipient.ID);

                        if (receiverOnline)
                        {
                            if (senderOnline)
                            {
                                OnlineNotificationPacket notify = new OnlineNotificationPacket();
                                notify.AgentBlock = new OnlineNotificationPacket.AgentBlockBlock[0];
                                notify.AgentBlock[0] = new OnlineNotificationPacket.AgentBlockBlock();
                                notify.AgentBlock[0].AgentID = agent.ID;
                                server.UDP.SendPacket(recipient.ID, notify, PacketCategory.State);
                            }
                            else
                            {
                                OfflineNotificationPacket notify = new OfflineNotificationPacket();
                                notify.AgentBlock = new OfflineNotificationPacket.AgentBlockBlock[0];
                                notify.AgentBlock[0] = new OfflineNotificationPacket.AgentBlockBlock();
                                notify.AgentBlock[0].AgentID = agent.ID;
                                server.UDP.SendPacket(recipient.ID, notify, PacketCategory.State);
                            }
                        }

                        if (senderOnline)
                        {
                            if (receiverOnline)
                            {
                                OnlineNotificationPacket notify = new OnlineNotificationPacket();
                                notify.AgentBlock = new OnlineNotificationPacket.AgentBlockBlock[0];
                                notify.AgentBlock[0] = new OnlineNotificationPacket.AgentBlockBlock();
                                notify.AgentBlock[0].AgentID = recipient.ID;
                                server.UDP.SendPacket(agent.ID, notify, PacketCategory.State);
                            }
                            else
                            {
                                OfflineNotificationPacket notify = new OfflineNotificationPacket();
                                notify.AgentBlock = new OfflineNotificationPacket.AgentBlockBlock[0];
                                notify.AgentBlock[0] = new OfflineNotificationPacket.AgentBlockBlock();
                                notify.AgentBlock[0].AgentID = recipient.ID;
                                server.UDP.SendPacket(agent.ID, notify, PacketCategory.State);
                            }
                        }
                    }
                }
            }
        }

    }
}
