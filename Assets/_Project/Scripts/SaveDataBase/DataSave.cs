using Newtonsoft.Json;
using System;

namespace Asteroid.Database
{
    [Serializable]
    public class DataSave
    {
        [JsonProperty] private string _name;
        [JsonProperty] private int _countSummaryEnemiesDestroyed;
        [JsonProperty] private bool _isLaserUsed;
        [JsonProperty] private int _countCoins;
        [JsonProperty] private bool _adsDisabled;
        [JsonProperty] private DateTime _lastSaveTime;

        public object this[string key]
        {
            get
            {
                return key switch
                {
                    KeyData.DEAD_ENEMIES_COUNT_SUMMARY => _countSummaryEnemiesDestroyed,
                    KeyData.COINS_COUNT => _countCoins,
                    KeyData.ADS_DISABLED => _adsDisabled,
                    KeyData.IS_LASER_USED => _isLaserUsed,
                    KeyData.NAME=>_name,
                    KeyData.LAST_SAVE_TIME => _lastSaveTime,
                    _ => throw new ArgumentException($"Invalid key: {key}")
                };
            }
            set
            {
                switch (key)
                {
                    case KeyData.DEAD_ENEMIES_COUNT_SUMMARY:
                        _countSummaryEnemiesDestroyed = Convert.ToInt32(value);
                        break;
                    case KeyData.COINS_COUNT:
                        _countCoins = Convert.ToInt32(value);
                        break;
                    case KeyData.ADS_DISABLED:
                        _adsDisabled = Convert.ToBoolean(value);
                        break;
                    case KeyData.IS_LASER_USED:
                        _isLaserUsed = Convert.ToBoolean(_isLaserUsed);
                        break;
                    case KeyData.NAME:
                        _name = Convert.ToString(value);
                        break;
                    case KeyData.LAST_SAVE_TIME:
                        _lastSaveTime = (DateTime)value;    
                        break;
                    default:
                        throw new ArgumentException($"Invalid key: {key}");
                }
            }
        }
    }
}