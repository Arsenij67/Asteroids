
using Asteroid.Database;
using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataPresenter: BaseSaveDataPresenter, ISaveStrategy
    {
        public bool NoAdsStatus => (bool)(_dataSave[KeyData.ADS_DISABLED] ?? false);
        public int CountCoins => (int)(_dataSave[KeyData.COINS_COUNT] ?? 0);

        private IRemoteSavable _remoteSavable;
        public void Initialize(DataSave dataSave, ShopUI shopUI=null, IRemoteSavable remoteSavable = null)
        {
            base.Initialize(dataSave,shopUI);
            _remoteSavable = remoteSavable;   
        }

        public async UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            int oldEnemies = await _remoteSavable.GetKey<int>(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            _dataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = oldEnemies + enemiesToAdd;
            await _remoteSavable.SaveKey(KeyData.DEAD_ENEMIES_COUNT_SUMMARY, _dataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            UpdateLastSaveTime(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
        }

        public async UniTask AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            _dataSave[KeyData.COINS_COUNT] = oldCoins + coinsToAdd;
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, _dataSave[KeyData.COINS_COUNT]);
            UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public void UpdateNoAdsStatus(bool advertisementIsCanceled)
        {
            _dataSave[KeyData.ADS_DISABLED] = advertisementIsCanceled;
            _remoteSavable.SaveKey(KeyData.ADS_DISABLED, _dataSave[KeyData.ADS_DISABLED]);
        }

        public async UniTask RemoveCountCoins(int coinsToRemove)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, oldCoins - coinsToRemove);
            UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public SaveChoice GetMode()
        {
            return SaveChoice.UseCloud;
        }

        public void UpdateLastSaveTime(string key)
        {
            _dataSave[KeyData.LAST_SAVE_TIME] = (DateTime)_remoteSavable.GetKeyLastModified(key);
        }

    }
}
