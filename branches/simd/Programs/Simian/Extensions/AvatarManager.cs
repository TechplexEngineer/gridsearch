using System;
using System.Collections.Generic;
using System.Threading;
using ExtensionLoader;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using OpenMetaverse.Packets;

namespace Simian.Extensions
{
    class AvatarManager : IExtension<Simian>, IAvatarProvider
    {
        Simian server;
        int currentWearablesSerialNum = -1;
        int currentAnimSequenceNum = 0;
        Timer CoarseLocationTimer;

        public AvatarManager()
        {
        }

        public void Start(Simian server)
        {
            this.server = server;

            server.UDP.RegisterPacketCallback(PacketType.AvatarPropertiesRequest, new PacketCallback(AvatarPropertiesRequestHandler));
            server.UDP.RegisterPacketCallback(PacketType.AgentWearablesRequest, new PacketCallback(AgentWearablesRequestHandler));
            server.UDP.RegisterPacketCallback(PacketType.AgentIsNowWearing, new PacketCallback(AgentIsNowWearingHandler));
            server.UDP.RegisterPacketCallback(PacketType.AgentSetAppearance, new PacketCallback(AgentSetAppearanceHandler));
            server.UDP.RegisterPacketCallback(PacketType.AgentCachedTexture, new PacketCallback(AgentCachedTextureHandler));
            server.UDP.RegisterPacketCallback(PacketType.AgentAnimation, new PacketCallback(AgentAnimationHandler));
            server.UDP.RegisterPacketCallback(PacketType.SoundTrigger, new PacketCallback(SoundTriggerHandler));
            server.UDP.RegisterPacketCallback(PacketType.ViewerEffect, new PacketCallback(ViewerEffectHandler));
            server.UDP.RegisterPacketCallback(PacketType.UUIDNameRequest, new PacketCallback(GuidNameRequestHandler));

            if (CoarseLocationTimer != null) CoarseLocationTimer.Dispose();
            CoarseLocationTimer = new Timer(CoarseLocationTimer_Elapsed);
            CoarseLocationTimer.Change(1000, 1000);
        }

        public void Stop()
        {
            if (CoarseLocationTimer != null)
            {
                CoarseLocationTimer.Dispose();
                CoarseLocationTimer = null;
            }
        }

        public bool SetDefaultAnimation(Agent agent, Guid animID)
        {
            return agent.Animations.SetDefaultAnimation(animID, ref currentAnimSequenceNum);
        }

        public bool AddAnimation(Agent agent, Guid animID)
        {
            return agent.Animations.Add(animID, ref currentAnimSequenceNum);
        }

        public bool RemoveAnimation(Agent agent, Guid animID)
        {
            return agent.Animations.Remove(animID);
        }

        public void SendAnimations(Agent agent)
        {
            AvatarAnimationPacket sendAnim = new AvatarAnimationPacket();
            sendAnim.Sender.ID = agent.AgentID;
            sendAnim.AnimationSourceList = new AvatarAnimationPacket.AnimationSourceListBlock[1];
            sendAnim.AnimationSourceList[0] = new AvatarAnimationPacket.AnimationSourceListBlock();
            sendAnim.AnimationSourceList[0].ObjectID = agent.AgentID;

            Guid[] animIDS;
            int[] sequenceNums;
            agent.Animations.GetArrays(out animIDS, out sequenceNums);

            sendAnim.AnimationList = new AvatarAnimationPacket.AnimationListBlock[animIDS.Length];
            for (int i = 0; i < animIDS.Length; i++)
            {
                sendAnim.AnimationList[i] = new AvatarAnimationPacket.AnimationListBlock();
                sendAnim.AnimationList[i].AnimID = animIDS[i];
                sendAnim.AnimationList[i].AnimSequenceID = sequenceNums[i];
            }

            server.UDP.BroadcastPacket(sendAnim, PacketCategory.State);
        }

        public void TriggerSound(Agent agent, Guid soundID, float gain)
        {
            SoundTriggerPacket sound = new SoundTriggerPacket();
            sound.SoundData.Handle = server.RegionHandle;
            sound.SoundData.ObjectID = agent.AgentID;
            sound.SoundData.ParentID = agent.AgentID;
            sound.SoundData.OwnerID = agent.AgentID;
            sound.SoundData.Position = agent.Avatar.Position;
            sound.SoundData.SoundID = soundID;
            sound.SoundData.Gain = gain;

            server.UDP.BroadcastPacket(sound, PacketCategory.State);
        }

        void AgentAnimationHandler(Packet packet, Agent agent)
        {
            AgentAnimationPacket animPacket = (AgentAnimationPacket)packet;
            bool changed = false;

            for (int i = 0; i < animPacket.AnimationList.Length; i++)
            {
                AgentAnimationPacket.AnimationListBlock block = animPacket.AnimationList[i];

                if (block.StartAnim)
                {
                    if (agent.Animations.Add(block.AnimID, ref currentAnimSequenceNum))
                        changed = true;
                }
                else
                {
                    if (agent.Animations.Remove(block.AnimID))
                        changed = true;
                }
            }

            if (changed)
                SendAnimations(agent);
        }

        void ViewerEffectHandler(Packet packet, Agent agent)
        {
            ViewerEffectPacket effect = (ViewerEffectPacket)packet;

            effect.AgentData.AgentID = Guid.Empty;
            effect.AgentData.SessionID = Guid.Empty;

            server.UDP.BroadcastPacket(effect, PacketCategory.State);
        }

        void AvatarPropertiesRequestHandler(Packet packet, Agent agent)
        {
            AvatarPropertiesRequestPacket request = (AvatarPropertiesRequestPacket)packet;

            Agent foundAgent;
            if (server.Agents.TryGetValue(request.AgentData.AvatarID, out foundAgent))
            {
                AvatarPropertiesReplyPacket reply = new AvatarPropertiesReplyPacket();
                reply.AgentData.AgentID = agent.AgentID;
                reply.AgentData.AvatarID = request.AgentData.AvatarID;
                reply.PropertiesData.AboutText = Utils.StringToBytes(foundAgent.ProfileAboutText);
                reply.PropertiesData.BornOn = Utils.StringToBytes(foundAgent.ProfileBornOn);
                reply.PropertiesData.CharterMember = new byte[1];
                reply.PropertiesData.FLAboutText = Utils.StringToBytes(foundAgent.ProfileFirstText);
                reply.PropertiesData.Flags = (uint)foundAgent.ProfileFlags;
                reply.PropertiesData.FLImageID = foundAgent.ProfileFirstImage;
                reply.PropertiesData.ImageID = foundAgent.ProfileImage;
                reply.PropertiesData.PartnerID = foundAgent.PartnerID;
                reply.PropertiesData.ProfileURL = Utils.StringToBytes(foundAgent.ProfileURL);

                server.UDP.SendPacket(agent.AgentID, reply, PacketCategory.Transaction);
            }
            else
            {
                Logger.Log("AvatarPropertiesRequest for unknown agent " + request.AgentData.AvatarID.ToString(),
                    Helpers.LogLevel.Warning);
            }
        }

        bool TryAddWearable(Guid agentID, Dictionary<WearableType, InventoryItem> wearables, WearableType type, Guid itemID)
        {
            InventoryObject obj;
            if (itemID != Guid.Empty && server.Inventory.TryGetInventory(agentID, itemID, out obj) &&
                obj is InventoryItem)
            {
                wearables.Add(type, (InventoryItem)obj);
                return true;
            }
            else
            {
                return false;
            }
        }

        Dictionary<WearableType, InventoryItem> GetCurrentWearables(Agent agent)
        {
            Dictionary<WearableType, InventoryItem> wearables = new Dictionary<WearableType, InventoryItem>();

            TryAddWearable(agent.AgentID, wearables, WearableType.Shape, agent.ShapeItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Skin, agent.SkinItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Hair, agent.HairItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Eyes, agent.EyesItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Shirt, agent.ShirtItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Pants, agent.PantsItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Shoes, agent.ShoesItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Socks, agent.SocksItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Jacket, agent.JacketItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Gloves, agent.GlovesItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Undershirt, agent.UndershirtItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Underpants, agent.UnderpantsItem);
            TryAddWearable(agent.AgentID, wearables, WearableType.Skirt, agent.SkirtItem);

            return wearables;
        }

        void SendWearables(Agent agent)
        {
            AgentWearablesUpdatePacket update = new AgentWearablesUpdatePacket();
            update.AgentData.AgentID = agent.AgentID;

            Dictionary<WearableType, InventoryItem> wearables = GetCurrentWearables(agent);
            update.WearableData = new AgentWearablesUpdatePacket.WearableDataBlock[wearables.Count];
            int i = 0;

            foreach (KeyValuePair<WearableType, InventoryItem> kvp in wearables)
            {
                update.WearableData[i] = new AgentWearablesUpdatePacket.WearableDataBlock();
                update.WearableData[i].AssetID = kvp.Value.AssetID;
                update.WearableData[i].ItemID = kvp.Value.ID;
                update.WearableData[i].WearableType = (byte)kvp.Key;
                ++i;
            }

            // Technically this should be per-agent, but if the only requirement is that it
            // increments this is easier
            update.AgentData.SerialNum = (uint)Interlocked.Increment(ref currentWearablesSerialNum);

            Logger.DebugLog(String.Format("Sending info about {0} wearables", wearables.Count));

            server.UDP.SendPacket(agent.AgentID, update, PacketCategory.Asset);
        }

        void AgentWearablesRequestHandler(Packet packet, Agent agent)
        {
            SendWearables(agent);
        }

        void AgentIsNowWearingHandler(Packet packet, Agent agent)
        {
            AgentIsNowWearingPacket wearing = (AgentIsNowWearingPacket)packet;

            for (int i = 0; i < wearing.WearableData.Length; i++)
            {
                Guid itemID = wearing.WearableData[i].ItemID;

                #region Update Wearables

                switch ((WearableType)wearing.WearableData[i].WearableType)
                {
                    case WearableType.Shape:
                        agent.ShapeItem = itemID;
                        break;
                    case WearableType.Skin:
                        agent.SkinItem = itemID;
                        break;
                    case WearableType.Hair:
                        agent.HairItem = itemID;
                        break;
                    case WearableType.Eyes:
                        agent.EyesItem = itemID;
                        break;
                    case WearableType.Shirt:
                        agent.ShirtItem = itemID;
                        break;
                    case WearableType.Pants:
                        agent.PantsItem = itemID;
                        break;
                    case WearableType.Shoes:
                        agent.ShoesItem = itemID;
                        break;
                    case WearableType.Socks:
                        agent.SocksItem = itemID;
                        break;
                    case WearableType.Jacket:
                        agent.JacketItem = itemID;
                        break;
                    case WearableType.Gloves:
                        agent.GlovesItem = itemID;
                        break;
                    case WearableType.Undershirt:
                        agent.UndershirtItem = itemID;
                        break;
                    case WearableType.Underpants:
                        agent.UnderpantsItem = itemID;
                        break;
                    case WearableType.Skirt:
                        agent.SkirtItem = itemID;
                        break;
                }

                #endregion Update Wearables
            }

            // FIXME: GetCurrentWearables() is a very expensive call, remove it from this debug line
            Logger.DebugLog("Updated agent wearables, new count: " + GetCurrentWearables(agent).Count);
        }

        void AgentSetAppearanceHandler(Packet packet, Agent agent)
        {
            AgentSetAppearancePacket set = (AgentSetAppearancePacket)packet;

            Logger.DebugLog("Updating avatar appearance");

            //TODO: Store this for cached bake responses
            for (int i = 0; i < set.WearableData.Length; i++)
            {
                AppearanceManager.TextureIndex index = (AppearanceManager.TextureIndex)set.WearableData[i].TextureIndex;
                Guid cacheID = set.WearableData[i].CacheID;

                Logger.DebugLog(String.Format("WearableData: {0} is now {1}", index, cacheID));
            }

            // Create a TextureEntry
            Primitive.TextureEntry textureEntry = new Primitive.TextureEntry(set.ObjectData.TextureEntry, 0,
                set.ObjectData.TextureEntry.Length);

            // Create a block of VisualParams
            byte[] visualParams = new byte[set.VisualParam.Length];
            for (int i = 0; i < set.VisualParam.Length; i++)
                visualParams[i] = set.VisualParam[i].ParamValue;

            server.Scene.AvatarAppearance(this, agent, textureEntry, visualParams);
        }

        void AgentCachedTextureHandler(Packet packet, Agent agent)
        {
            AgentCachedTexturePacket cached = (AgentCachedTexturePacket)packet;

            AgentCachedTextureResponsePacket response = new AgentCachedTextureResponsePacket();
            response.AgentData.AgentID = agent.AgentID;
            response.AgentData.SerialNum = cached.AgentData.SerialNum;

            response.WearableData = new AgentCachedTextureResponsePacket.WearableDataBlock[cached.WearableData.Length];

            // TODO: Respond back with actual cache entries if we have them
            for (int i = 0; i < cached.WearableData.Length; i++)
            {
                response.WearableData[i] = new AgentCachedTextureResponsePacket.WearableDataBlock();
                response.WearableData[i].TextureIndex = cached.WearableData[i].TextureIndex;
                response.WearableData[i].TextureID = Guid.Empty;
                response.WearableData[i].HostName = new byte[0];
            }

            response.Header.Zerocoded = true;

            server.UDP.SendPacket(agent.AgentID, response, PacketCategory.Transaction);
        }

        void SoundTriggerHandler(Packet packet, Agent agent)
        {
            SoundTriggerPacket trigger = (SoundTriggerPacket)packet;
            TriggerSound(agent, trigger.SoundData.SoundID, trigger.SoundData.Gain);
        }

        void GuidNameRequestHandler(Packet packet, Agent agent)
        {
            UUIDNameRequestPacket request = (UUIDNameRequestPacket)packet;

            GuidNameReplyPacket reply = new GuidNameReplyPacket();
            reply.GuidNameBlock = new GuidNameReplyPacket.GuidNameBlockBlock[request.GuidNameBlock.Length];

            for (int i = 0; i < request.GuidNameBlock.Length; i++)
            {
                Guid id = request.GuidNameBlock[i].ID;

                reply.GuidNameBlock[i] = new GuidNameReplyPacket.GuidNameBlockBlock();
                reply.GuidNameBlock[i].ID = id;

                Agent foundAgent;
                if (server.Agents.TryGetValue(id, out foundAgent))
                {
                    reply.GuidNameBlock[i].FirstName = Utils.StringToBytes(foundAgent.FirstName);
                    reply.GuidNameBlock[i].LastName = Utils.StringToBytes(foundAgent.LastName);
                }
                else
                {
                    reply.GuidNameBlock[i].FirstName = new byte[0];
                    reply.GuidNameBlock[i].LastName = new byte[0];
                }
            }

            server.UDP.SendPacket(agent.AgentID, reply, PacketCategory.Transaction);
        }

        void CoarseLocationTimer_Elapsed(object sender)
        {
            lock (server.Agents)
            {
                foreach (Agent recipient in server.Agents.Values)
                {
                    int i = 0;

                    CoarseLocationUpdatePacket update = new CoarseLocationUpdatePacket();
                    update.Index.Prey = -1;
                    update.Index.You = 0;

                    update.AgentData = new CoarseLocationUpdatePacket.AgentDataBlock[server.Agents.Count];
                    update.Location = new CoarseLocationUpdatePacket.LocationBlock[server.Agents.Count];

                    // Fill in this avatar
                    update.AgentData[0] = new CoarseLocationUpdatePacket.AgentDataBlock();
                    update.AgentData[0].AgentID = recipient.AgentID;
                    update.Location[0] = new CoarseLocationUpdatePacket.LocationBlock();
                    update.Location[0].X = (byte)((int)recipient.Avatar.Position.X);
                    update.Location[0].Y = (byte)((int)recipient.Avatar.Position.Y);
                    update.Location[0].Z = (byte)((int)recipient.Avatar.Position.Z / 4);
                    ++i;

                    foreach (Agent agent in server.Agents.Values)
                    {
                        if (agent != recipient)
                        {
                            update.AgentData[i] = new CoarseLocationUpdatePacket.AgentDataBlock();
                            update.AgentData[i].AgentID = agent.AgentID;
                            update.Location[i] = new CoarseLocationUpdatePacket.LocationBlock();
                            update.Location[i].X = (byte)((int)agent.Avatar.Position.X);
                            update.Location[i].Y = (byte)((int)agent.Avatar.Position.Y);
                            update.Location[i].Z = (byte)((int)agent.Avatar.Position.Z / 4);
                            ++i;
                        }
                    }

                    server.UDP.SendPacket(recipient.AgentID, update, PacketCategory.State);
                }
            }
        }
    }
}
