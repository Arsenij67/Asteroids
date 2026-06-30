using System;

namespace Asteroid.Database
{
    [Serializable]
    public class DataSave
    {
        private string _name;
        private int _countSummaryEnemiesDestroyed;
        private bool _isLaserUsed;
        private int _countCoins;
        private bool _adsDisabled;

        public object this[string key]
        {
            get
            {
                return key switch
                {
                    CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY => _countSummaryEnemiesDestroyed,
                    CloudKeyData.COINS_COUNT => _countCoins,
                    CloudKeyData.ADS_DISABLED => _adsDisabled,
                    CloudKeyData.IS_LASER_USED => _isLaserUsed,
                    CloudKeyData.NAME=>_name,
                    _ => throw new ArgumentException($"Invalid key: {key}")
                };
            }
            set
            {
                switch (key)
                {
                    case CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY:
                        _countSummaryEnemiesDestroyed = Convert.ToInt32(value);
                        break;
                    case CloudKeyData.COINS_COUNT:
                        _countCoins = Convert.ToInt32(value);
                        break;
                    case CloudKeyData.ADS_DISABLED:
                        _adsDisabled = Convert.ToBoolean(value);
                        break;
                    case CloudKeyData.IS_LASER_USED:
                        _isLaserUsed = Convert.ToBoolean(_isLaserUsed);
                        break;
                    case CloudKeyData.NAME:
                        _name = Convert.ToString(value);
                        break;
                    default:
                        throw new ArgumentException($"Invalid key: {key}");
                }
            }
        }

    }
}