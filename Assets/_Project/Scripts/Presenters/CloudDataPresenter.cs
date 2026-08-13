
using Asteroid.Database;
using Asteroid.Database.Connection;
using Asteroid.Generation;
using Asteroid.SpaceShip;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataPresenter: SaveStrategy
    {
        private IRemoteSavable _remoteSavable;
        private WIFIConnector _wifiConnector;

        public UniTask Initialize(WIFIConnector wIFIConnector,DataSave dataSave,InstanceCreator instanceLoader, IRemoteSavable remoteSavable, ShopView shopUI = null, GameOverPresenter shipStatisticsPresenter = null)
        {
            base.Initialize(dataSave,instanceLoader,shopUI,shipStatisticsPresenter);
            _remoteSavable = remoteSavable;
            _wifiConnector = wIFIConnector;
            return UpdateLastSaveTime();
        }

        public override async UniTask AddCountDeadEnemies(int enemiesToAdd)
         {
            if(enemiesToAdd<=0) return ;

            int oldEnemies = await _remoteSavable.GetKey<int>(KeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = oldEnemies + enemiesToAdd;
            if(!_remoteSavable.IsInitialized)
            {
               await _remoteSavable.Initialize(DataSave,_wifiConnector);
            }
            await _remoteSavable.SaveKey(KeyData.DEAD_ENEMIES_COUNT_SUMMARY, DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            await UpdateLastSaveTime();
        }

        public override async UniTask AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            DataSave[KeyData.COINS_COUNT] = oldCoins + coinsToAdd;
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave,_wifiConnector);
            }
            Debug.Log("Обновили" + oldCoins +" "+ coinsToAdd);
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, DataSave[KeyData.COINS_COUNT]);
            await UpdateLastSaveTime();
        }

        public override async UniTask UpdateNoAdsStatus(bool advertisementIsCanceled)
        {
            DataSave[KeyData.ADS_DISABLED] = advertisementIsCanceled;
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave, _wifiConnector);
            }
           await _remoteSavable.SaveKey(KeyData.ADS_DISABLED, DataSave[KeyData.ADS_DISABLED]);
        }

        public override async UniTask RemoveCountCoins(int coinsToRemove)
        {
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave, _wifiConnector);
            }
            int oldCoins = await _remoteSavable.GetKey<int>(KeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(KeyData.COINS_COUNT, oldCoins - coinsToRemove);
            await UpdateLastSaveTime();
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseCloud;
        }

        protected override async UniTask UpdateLastSaveTime()
        {
            if (!_remoteSavable.IsInitialized)
            {
                await _remoteSavable.Initialize(DataSave, _wifiConnector);
            }
            DataSave[KeyData.LAST_SAVE_TIME] = await _remoteSavable.GetTimeLastModified();
        }
    }
}
