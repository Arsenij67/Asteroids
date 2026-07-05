
using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataController
    {
        public int CountCoins=> _dataSave[CloudKeyData.COINS_COUNT]!=null ? (int)_dataSave[CloudKeyData.COINS_COUNT] : 0;
        public bool NoAdsStatus => (bool)_dataSave[CloudKeyData.ADS_DISABLED];

        private IRemoteSavable _remoteSavable;
        private DataSave _dataSave;
        public void Initialize(IRemoteSavable remoteSavable, DataSave dataSave)
        { 
            _remoteSavable = remoteSavable;   
            _dataSave = dataSave;
        }
        public async void AddCountDeadEnemies(int enemiesToAdd)
        {
            int oldEnemies = await _remoteSavable.GetKey<int>(CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            _dataSave[CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY] = oldEnemies + enemiesToAdd;
            await _remoteSavable.SaveKey(CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY, _dataSave[CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
        }
        public async UniTask AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(CloudKeyData.COINS_COUNT);
            _dataSave[CloudKeyData.COINS_COUNT] = oldCoins + coinsToAdd;
            await _remoteSavable.SaveKey(CloudKeyData.COINS_COUNT, _dataSave[CloudKeyData.COINS_COUNT]);
        }
        public void UpdateNoAdsStatusCloud(bool adsDisabled)
        {
            _dataSave[CloudKeyData.ADS_DISABLED] = adsDisabled;
            _remoteSavable.SaveKey(CloudKeyData.ADS_DISABLED, _dataSave[CloudKeyData.ADS_DISABLED]);
        }
        public async void RemoveCountCoins(int coinsToRemove)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(CloudKeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(CloudKeyData.COINS_COUNT, oldCoins - coinsToRemove);
        }

    }
}
