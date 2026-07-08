
using Asteroid.Database;
using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataPresenter: SaveStrategy
    {
        private IRemoteSavable _remoteSavable;

        public override void Initialize(DataSave dataSave, ShopUI shopUI = null,IRemoteSavable remoteSavable=null)
        {
            base.Initialize(dataSave, shopUI);
            _remoteSavable = remoteSavable;
        }

        public override async UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            int oldEnemies = await _remoteSavable.GetKey<int>(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = oldEnemies + enemiesToAdd;
            await _remoteSavable.SaveKey(KeyData.DEAD_ENEMIES_COUNT_SUMMARY, DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            UpdateLastSaveTime(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
        }

        public override async UniTask AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            DataSave[KeyData.COINS_COUNT] = oldCoins + coinsToAdd;
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, DataSave[KeyData.COINS_COUNT]);
            UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public override void UpdateNoAdsStatus(bool advertisementIsCanceled)
        {
            DataSave[KeyData.ADS_DISABLED] = advertisementIsCanceled;
            _remoteSavable.SaveKey(KeyData.ADS_DISABLED, DataSave[KeyData.ADS_DISABLED]);
        }

        public override async UniTask RemoveCountCoins(int coinsToRemove)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, oldCoins - coinsToRemove);
            UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseCloud;
        }

        public override void UpdateLastSaveTime(string key)
        {
            DataSave[KeyData.LAST_SAVE_TIME] = (DateTime)_remoteSavable.GetKeyLastModified(key);
        }

    }
}
