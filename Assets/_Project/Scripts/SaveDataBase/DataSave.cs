using System;

namespace Asteroid.Database
{
    [Serializable]
    public class DataSave
    {
        public string Name;
        public int CountEnemiesDestroyed;
        public bool IsLaserUsed;
        public int CountCoins;
        public bool AdsDisabled = false;
        public object this[string key]
        {
            get
            {
                return key switch
                {
                    CloudKeyData.DEAD_ENEMIES_COUNT => CountEnemiesDestroyed,
                    CloudKeyData.COINS_COUNT => CountCoins,
                    _ => throw new ArgumentException($"Invalid key: {key}")
                };
            }
            set
            {
                switch (key)
                {
                    case CloudKeyData.DEAD_ENEMIES_COUNT:
                        CountEnemiesDestroyed = (int)value;
                        break;
                    case CloudKeyData.COINS_COUNT:
                        CountCoins = (int)value;
                        break;
                    default:
                        throw new ArgumentException($"Invalid key: {key}");
                }
            }
        }

    }
}