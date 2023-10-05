using System;
namespace Game.Settings
{
    [Serializable]
    public class GameSetting
    {
        public bool IsMusicOn;
        public bool IsSoundOn;
        public bool IsVibrationOn;
        public float MasterVolume;
        public GameSetting()
        {
            IsMusicOn = true;
            IsSoundOn = true;
            IsVibrationOn = true;
            MasterVolume = 1;
        }
    }
}
