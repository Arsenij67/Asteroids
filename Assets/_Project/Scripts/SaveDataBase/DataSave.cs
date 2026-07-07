using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

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
        private DateTime _lastSaveTime;

        [NonSerialized]
        private List<object> _dataValues;
        public DataSave()
        {
            _dataValues = new List<object>() { _name,_countSummaryEnemiesDestroyed,_countSummaryEnemiesDestroyed,_isLaserUsed,_countCoins,_adsDisabled,_lastSaveTime };
        }
        

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