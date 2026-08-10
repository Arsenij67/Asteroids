using Asteroid.Generation;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database
{
    public abstract class SaveStrategy
    {
        public bool NoAdsStatus => (bool)(DataSave[KeyData.ADS_DISABLED] ?? false);
        public int CountCoins => (int)(DataSave[KeyData.COINS_COUNT] ?? 0);
        public DateTime LastSaveTime => (DateTime)(DataSave[KeyData.LAST_SAVE_TIME]?? DateTime.MinValue);
        public int CountDeadEnemies => (int)(DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] ?? 0);

        protected ShopView? ShopView;

        protected DataSave DataSave
        {
            get => _dataSave ?? _instanceCreator.CreateInstance<DataSave>();
            set => _dataSave = value;   
        }

        private InstanceCreator _instanceCreator;
        private DataSave _dataSave;
        private GameOverPresenter? _shipStatisticsPresenter;

        public void Initialize(DataSave dataSave, InstanceCreator instanceCreator, ShopView shopUI = null, GameOverPresenter shipStatisticsPresenter=null)
        {
            ShopView = shopUI;
            _dataSave = dataSave;
            _instanceCreator = instanceCreator; 
            _shipStatisticsPresenter = shipStatisticsPresenter;
        }

        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  UniTask UpdateNoAdsStatus(bool advertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        protected abstract UniTask UpdateLastSaveTime();
        private async UniTask UpdateCountCoins(int coinsSummary)
        {
            await AddCountCoins(coinsSummary-CountCoins);
        }

        private async UniTask UpdateDeadEnemies(int deadEnemiesSummary)
        {
            await AddCountDeadEnemies (deadEnemiesSummary - CountDeadEnemies);
        }

        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            if (ShopView != null)
            {
                ShopView.UpdateViewNoAds(isAdvertisementCanceled);
            }
        }

        public void UpdateUICountCoins(int countSummaryCoins)
        {
            if (ShopView != null)
            {
                ShopView.UpdateCountCoins(countSummaryCoins);
            }
        }

        public void UpdateUIDeadEnemies()
        {
            _shipStatisticsPresenter?.UpdateDestroyedEnemiesUI();
        }

        public void UpdateAllDataUI()
        {
            UpdateUINoAds(NoAdsStatus);
            UpdateUICountCoins(CountCoins);
            UpdateUIDeadEnemies();
        }

        public async UniTask UpdateAllData(DataSave dataSave)
        {
            await UpdateCountCoins((int)dataSave[KeyData.COINS_COUNT]);
            await UpdateDeadEnemies((int)dataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            await UpdateNoAdsStatus((bool)dataSave[KeyData.ADS_DISABLED]);
        }

    }
}
