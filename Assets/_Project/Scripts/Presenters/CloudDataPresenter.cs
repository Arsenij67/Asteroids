
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

        public void Initialize(DataSave dataSave,IInstanceLoader instanceLoader, IRemoteSavable remoteSavable, ShopUI shopUI = null)
        {
            base.Initialize(dataSave,instanceLoader,shopUI);
            _remoteSavable = remoteSavable;
        }

        public override async UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            int oldEnemies = await _remoteSavable.GetKey<int>(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = oldEnemies + enemiesToAdd;
            if(!_remoteSavable.IsInitialized)
            {
               await _remoteSavable.Initialize(DataSave);
            }
            await _remoteSavable.SaveKey(KeyData.DEAD_ENEMIES_COUNT_SUMMARY, DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            UpdateLastSaveTime(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
        }

        public override async UniTask AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            DataSave[KeyData.COINS_COUNT] = oldCoins + coinsToAdd;
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave);
            }
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, DataSave[KeyData.COINS_COUNT]);
            UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public override async UniTask UpdateNoAdsStatus(bool advertisementIsCanceled)
        {
            DataSave[KeyData.ADS_DISABLED] = advertisementIsCanceled;
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave);
            }
           await _remoteSavable.SaveKey(KeyData.ADS_DISABLED, DataSave[KeyData.ADS_DISABLED]);
        }

        public override async UniTask RemoveCountCoins(int coinsToRemove)
        {
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave);
            }
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, oldCoins - coinsToRemove);
            await UpdateLastSaveTime(KeyData.COINS_COUNT);
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseCloud;
        }

        protected override async UniTask UpdateLastSaveTime(string key)
        {
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave);
            }
            DataSave[KeyData.LAST_SAVE_TIME] = (DateTime)_remoteSavable.GetKeyLastModified(key);
        }
    }
}
