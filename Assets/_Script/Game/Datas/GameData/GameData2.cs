using System;
using System.Collections.Generic;
using Unity.Mathematics;
using static Game.Craft.CraftStateSequence;

namespace Game.Datas
{
    [Serializable]
    public class GameData2
    {
        public string Version;
        public int Notes;
        public List<SongProgress> SongProgresses;
    }


    [Serializable]
    public class SongProgress
    {
        public int Id;
        public SongDifficulty Easy;
        public SongDifficulty Normal;
        public SongDifficulty Hard;
    }
    [Serializable]
    public class SongDifficulty
    {
        public int Difficulty;
        public int BestStar;
        public int BestScore;
    }
}