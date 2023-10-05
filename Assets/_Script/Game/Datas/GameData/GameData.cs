using System;
using System.Collections.Generic;
using Unity.Mathematics;
using static Game.Craft.CraftStateSequence;

namespace Game.Datas
{
    [Serializable]
    public class GameData
    {
        public List<Locker> LockedIndex;
        public int Coin;
    }

    [Serializable]
    public class Locker
    {
        public enum LockType
        {
            Reward = 0, Gold = 1
        }

        public PartID PartID;
        public LockType Type;
        public bool Unlocked;
        public int Index;
        public int GoldToUnlock;
    }
}