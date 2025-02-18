using System;
using System.Collections.Generic;
using System.Threading;
using OpenMetaverse;

namespace Simian
{
    public struct Animation
    {
        public Guid ID;
        public int SequenceNum;

        public Animation(Guid id, int sequenceNum)
        {
            ID = id;
            SequenceNum = sequenceNum;
        }
    }

    public class AnimationSet
    {
        private Animation defaultAnimation;
        private List<Animation> animations = new List<Animation>();

        public AnimationSet()
        {
            ResetDefaultAnimation();
        }

        public bool HasAnimation(Guid animID)
        {
            if (defaultAnimation.ID == animID)
                return true;

            lock (animations)
            {
                for (int i = 0; i < animations.Count; ++i)
                {
                    if (animations[i].ID == animID)
                        return true;
                }
            }

            return false;
        }

        public bool Add(Guid animID, ref int sequenceCounter)
        {
            lock (animations)
            {
                if (!HasAnimation(animID))
                {
                    int sequenceNum = Interlocked.Increment(ref sequenceCounter);
                    animations.Add(new Animation(animID, sequenceNum));
                    return true;
                }
            }

            return false;
        }

        public bool Add(Guid animID, int sequenceNum)
        {
            lock (animations)
            {
                if (!HasAnimation(animID))
                {
                    animations.Add(new Animation(animID, sequenceNum));
                    return true;
                }
            }

            return false;
        }

        public bool Remove(Guid animID)
        {
            if (defaultAnimation.ID == animID)
            {
                ResetDefaultAnimation();
                return true;
            }
            else if (HasAnimation(animID))
            {
                lock (animations)
                {
                    for (int i = 0; i < animations.Count; i++)
                    {
                        if (animations[i].ID == animID)
                        {
                            animations.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Clear()
        {
            ResetDefaultAnimation();
            lock (animations) animations.Clear();
        }

        public bool SetDefaultAnimation(Guid animID, ref int sequenceCounter)
        {
            if (defaultAnimation.ID != animID)
            {
                int sequenceNum = Interlocked.Increment(ref sequenceCounter);
                defaultAnimation = new Animation(animID, sequenceNum);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetDefaultAnimation(Guid animID, int sequenceNum)
        {
            if (defaultAnimation.ID != animID)
            {
                defaultAnimation = new Animation(animID, sequenceNum);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetArrays(out Guid[] animIDs, out int[] sequenceNums)
        {
            lock (animations)
            {
                animIDs = new Guid[animations.Count + 1];
                sequenceNums = new int[animations.Count + 1];

                animIDs[0] = defaultAnimation.ID;
                sequenceNums[0] = defaultAnimation.SequenceNum;

                for (int i = 0; i < animations.Count; ++i)
                {
                    animIDs[i + 1] = animations[i].ID;
                    sequenceNums[i + 1] = animations[i].SequenceNum;
                }
            }
        }

        protected bool ResetDefaultAnimation()
        {
            return SetDefaultAnimation(Animations.STAND, 1);
        }
    }
}
