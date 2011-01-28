/*
 * Copyright (c) 2006-2007, Second Life Reverse Engineering Team
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the Second Life Reverse Engineering Team nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Timers;
using System.Net;
using System.Collections.Generic;
using libsecondlife.Packets;

namespace libsecondlife
{
    /// <summary>
    /// Represents an avatar in Second Life (other than your own)
    /// </summary>
    public class Avatar : LLObject
    {
        /// <summary>
        /// Avatar profile flags
        /// </summary>
        [Flags]
        public enum ProfileFlags
        {
            ALLOW_PUBLISH = 1,
            MATURE_PUBLISH = 2,
            IDENTIFIED = 4,
            TRANSACTED = 8,
            ONLINE = 16
        }

        #region Avatar Structs

        /// <summary>
        /// Positive and negative ratings
        /// </summary>
        public struct Statistics
        {
            /// <summary>Positive ratings for Behavior</summary>
            public int BehaviorPositive;
            /// <summary>Negative ratings for Behavior</summary>
            public int BehaviorNegative;
            /// <summary>Positive ratings for Appearance</summary>
            public int AppearancePositive;
            /// <summary>Negative ratings for Appearance</summary>
            public int AppearanceNegative;
            /// <summary>Positive ratings for Building</summary>
            public int BuildingPositive;
            /// <summary>Negative ratings for Building</summary>
            public int BuildingNegative;
            /// <summary>Positive ratings given by this avatar</summary>
            public int GivenPositive;
            /// <summary>Negative ratings given by this avatar</summary>
            public int GivenNegative;
        }

        /// <summary>
        /// Avatar properties including about text, profile URL, image IDs and 
        /// publishing settings
        /// </summary>
        public struct AvatarProperties
        {
            /// <summary>First Life about text</summary>
            public string FirstLifeText;
            /// <summary>First Life image ID</summary>
            public LLUUID FirstLifeImage;
            /// <summary></summary>
            public LLUUID Partner;
            /// <summary></summary>
            public string AboutText;
            /// <summary></summary>
            public string BornOn;
            /// <summary></summary>
            public string CharterMember;
            /// <summary>Profile image ID</summary>
            public LLUUID ProfileImage;
            /// <summary>Flags of the profile</summary>
            public ProfileFlags Flags;
            /// <summary>Web URL for this profile</summary>
            public string ProfileURL;

            #region Properties

            /// <summary>Should this profile be published on the web</summary>
            public bool AllowPublish
            {
                get
                {
                    return ((Flags & ProfileFlags.ALLOW_PUBLISH) != 0);
                }
                set
                {
                    if (value == true)
                    {
                        Flags |= ProfileFlags.ALLOW_PUBLISH;
                    }
                    else
                    {
                        Flags &= ~ProfileFlags.ALLOW_PUBLISH;
                    }
                }
            }
            /// <summary>Avatar Online Status</summary>
            public bool Online
            {
                get
                {
                    return ((Flags & ProfileFlags.ONLINE) != 0);
                }
                set
                {
                    if (value == true)
                    {
                        Flags |= ProfileFlags.ONLINE;
                    }
                    else
                    {
                        Flags &= ~ProfileFlags.ONLINE;
                    }
                }
            }
            /// <summary>Is this a mature profile</summary>
            public bool MaturePublish
            {
                get
                {
                    return ((Flags & ProfileFlags.MATURE_PUBLISH) != 0);
                }
                set
                {
                    if (value == true)
                    {
                        Flags |= ProfileFlags.MATURE_PUBLISH;
                    }
                    else
                    {
                        Flags &= ~ProfileFlags.MATURE_PUBLISH;
                    }
                }
            }
            /// <summary></summary>
            public bool Identified
            {
                get
                {
                    return ((Flags & ProfileFlags.IDENTIFIED) != 0);
                }
                set
                {
                    if (value == true)
                    {
                        Flags |= ProfileFlags.IDENTIFIED;
                    }
                    else
                    {
                        Flags &= ~ProfileFlags.IDENTIFIED;
                    }
                }
            }
            ///// <summary></summary>
            public bool Transacted
            {
                get
                {
                    return ((Flags & ProfileFlags.TRANSACTED) != 0);
                }
                set
                {
                    if (value == true)
                    {
                        Flags |= ProfileFlags.TRANSACTED;
                    }
                    else
                    {
                        Flags &= ~ProfileFlags.TRANSACTED;
                    }
                }
            }

            #endregion Properties
        }
        
        /// <summary>
        /// Avatar interests including spoken languages, skills, and "want to"
        /// choices
        /// </summary>
        public struct Interests
        {
            /// <summary>Languages profile field</summary>
            public string LanguagesText;
            /// <summary></summary>
            // FIXME:
            public uint SkillsMask;
            /// <summary></summary>
            public string SkillsText;
            /// <summary></summary>
            // FIXME:
            public uint WantToMask;
            /// <summary></summary>
            public string WantToText;
        }

        #endregion Avatar Structs


        #region Public Members

        /// <summary>Groups that this avatar is a member of</summary>
        public List<LLUUID> Groups = new List<LLUUID>();
        /// <summary>Positive and negative ratings</summary>
        public Statistics ProfileStatistics = new Statistics();
        /// <summary>Avatar properties including about text, profile URL, image IDs and 
        /// publishing settings</summary>
        public AvatarProperties ProfileProperties = new AvatarProperties();
        /// <summary>Avatar interests including spoken languages, skills, and "want to"
        /// choices</summary>
        public Interests ProfileInterests = new Interests();
        /// <summary>Simulator the avatar is in</summary>
        public Simulator CurrentSim = null;

        #endregion Public Members


        /// <summary>Full name</summary>
        public string Name
        {
            get
            {
                if (name.Length > 0)
                {
                    return name;
                }
                else if (NameValues == null || NameValues.Length == 0)
                {
                    return String.Empty;
                }
                else
                {
                    string firstName = String.Empty;
                    string lastName = String.Empty;

                    for (int i = 0; i < NameValues.Length; i++)
                    {
                        if (NameValues[i].Name == "FirstName" && NameValues[i].Type == NameValue.ValueType.String)
                            firstName = (string)NameValues[i].Value;
                        else if (NameValues[i].Name == "LastName" && NameValues[i].Type == NameValue.ValueType.String)
                            lastName = (string)NameValues[i].Value;
                    }

                    if (firstName != String.Empty && lastName != String.Empty)
                    {
                        name = String.Format("{0} {1}", firstName, lastName);
                        return name;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
            }
        }

        /// <summary>Active group</summary>
        public string GroupName
        {
            get
            {
                for (int i = 0; i < NameValues.Length; i++)
                {
                    if (NameValues[i].Name == "Title" && NameValues[i].Type == NameValue.ValueType.String)
                    {
                        groupName = (string)NameValues[i].Value;
                        break;
                    }
                }

                return groupName;
            }
        }

        /// <summary>Gets the local ID of the prim the avatar is sitting on,
        /// zero if the avatar is not currently sitting</summary>
        public uint SittingOn { get { return sittingOn; } }


        internal string name = String.Empty;
        internal string groupName = String.Empty;
        internal uint sittingOn
        {
            get
            {
                return ParentID;
            }
 
            set
            {
                ParentID = value;
            }
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public Avatar()
        {
        }
    }
}
