using System;

namespace Asteroid.Database
{
    [Serializable]
    public class DataSave
    {
        public string Name;
        public int EnemiesDestroyed;
        public bool IsLaserUsed;
        public int CountCoins;
        public bool AdsDisabled;
    }
}
