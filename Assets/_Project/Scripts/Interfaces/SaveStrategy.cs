using Asteroid.Generation;
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
        public DateTime LastSaveTime => (DateTime)(DataSave[KeyData.LAST_SAVE_TIME] ?? DateTime.MinValue);

        public int CountDeadEnemies => (int)(DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] ?? 0);

        protected ShopUI ShopUI;

        protected DataSave DataSave
        {
            get => _dataSave ?? _instanceCreator.CreateInstance<DataSave>();
            set => _dataSave = value;   
        }

        private IInstanceLoader _instanceCreator;
        private DataSave _dataSave;

        public void Initialize(DataSave dataSave, IInstanceLoader instanceCreator, ShopUI shopUI = null)
        {
            ShopUI = shopUI;
            _dataSave = dataSave;
            _instanceCreator = instanceCreator; 
        }
        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  UniTask UpdateNoAdsStatus(bool advertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        protected abstract UniTask UpdateLastSaveTime(string key);
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
            ShopUI?.UpdateViewNoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            ShopUI?.UpdateCountCoins((int)DataSave[KeyData.COINS_COUNT]);
        }

        public void UpdateAllDataUI()
        {
            UpdateUINoAds(NoAdsStatus);
            UpdateUICountCoins(CountCoins);
        }

        public async UniTask UpdateAllData(DataSave dataSave)
        {
            await UpdateCountCoins((int)dataSave[KeyData.COINS_COUNT]);
            await UpdateDeadEnemies((int)dataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY]);
            await UpdateNoAdsStatus((bool)dataSave[KeyData.ADS_DISABLED]);
        }

    }
}
