using System;
using System.Collections.Generic;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace OpenMetaverse.TestClient
{
    public class CloneProfileCommand : Command
    {
        Avatar.AvatarProperties Properties;
        Avatar.Interests Interests;
        List<Guid> Groups = new List<Guid>();
        bool ReceivedProperties = false;
        bool ReceivedInterests = false;
        bool ReceivedGroups = false;
        ManualResetEvent ReceivedProfileEvent = new ManualResetEvent(false);

        public CloneProfileCommand(TestClient testClient)
        {
            testClient.Avatars.OnAvatarInterests += new AvatarManager.AvatarInterestsCallback(Avatars_OnAvatarInterests);
            testClient.Avatars.OnAvatarProperties += new AvatarManager.AvatarPropertiesCallback(Avatars_OnAvatarProperties);
            testClient.Avatars.OnAvatarGroups += new AvatarManager.AvatarGroupsCallback(Avatars_OnAvatarGroups);
            testClient.Groups.OnGroupJoined += new GroupManager.GroupJoinedCallback(Groups_OnGroupJoined);

            Name = "cloneprofile";
            Description = "Clones another avatars profile as closely as possible. WARNING: This command will " +
                "destroy your existing profile! Usage: cloneprofile [targetGuid]";
            Category = CommandCategory.Other;
        }

        public override string Execute(string[] args, Guid fromAgentID)
        {
            if (args.Length != 1)
                return Description;

            Guid targetID;
            ReceivedProperties = false;
            ReceivedInterests = false;
            ReceivedGroups = false;

            try
            {
                targetID = new Guid(args[0]);
            }
            catch (Exception)
            {
                return Description;
            }

            // Request all of the packets that make up an avatar profile
            Client.Avatars.RequestAvatarProperties(targetID);

            // Wait for all the packets to arrive
            ReceivedProfileEvent.Reset();
            ReceivedProfileEvent.WaitOne(5000, false);

            // Check if everything showed up
            if (!ReceivedInterests || !ReceivedProperties || !ReceivedGroups)
                return "Failed to retrieve a complete profile for that Guid";

            // Synchronize our profile
            Client.Self.UpdateInterests(Interests);
            Client.Self.UpdateProfile(Properties);

            // TODO: Leave all the groups we're currently a member of? This could
            // break TestClient connectivity that might be relying on group authentication

            // Attempt to join all the groups
            foreach (Guid groupID in Groups)
            {
                Client.Groups.RequestJoinGroup(groupID);
            }

            return "Synchronized our profile to the profile of " + targetID.ToString();
        }

        void Avatars_OnAvatarProperties(Guid avatarID, Avatar.AvatarProperties properties)
        {
            lock (ReceivedProfileEvent)
            {
                Properties = properties;
                ReceivedProperties = true;

                if (ReceivedInterests && ReceivedProperties && ReceivedGroups)
                    ReceivedProfileEvent.Set();
            }
        }

        void Avatars_OnAvatarInterests(Guid avatarID, Avatar.Interests interests)
        {
            lock (ReceivedProfileEvent)
            {
                Interests = interests;
                ReceivedInterests = true;

                if (ReceivedInterests && ReceivedProperties && ReceivedGroups)
                    ReceivedProfileEvent.Set();
            }
        }

        void Avatars_OnAvatarGroups(Guid avatarID, List<AvatarGroup> groups)
        {
            lock (ReceivedProfileEvent)
            {
                foreach (AvatarGroup group in groups)
                {
                    Groups.Add(group.GroupID);
                }

                ReceivedGroups = true;

                if (ReceivedInterests && ReceivedProperties && ReceivedGroups)
                    ReceivedProfileEvent.Set();
            }
        }
        
        void Groups_OnGroupJoined(Guid groupID, bool success)
        {
            Console.WriteLine(Client.ToString() + (success ? " joined " : " failed to join ") +
                groupID.ToString());

            if (success)
            {
                Console.WriteLine(Client.ToString() + " setting " + groupID.ToString() +
                    " as the active group");
                Client.Groups.ActivateGroup(groupID);
            }
        }
    }
}
