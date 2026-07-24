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

        protected ShopUI ShopUI;

        protected DataSave DataSave
        {
            get => _dataSave;
            set => _dataSave = value;
        }

        private IInstanceLoader _instanceCreator;
        private DataSave _dataSave;

        public void Initialize(DataSave dataSave, IInstanceLoader instanceCreator, ShopUI shopUI = null)
        {
            ShopUI = shopUI;
            _dataSave = dataSave;
            _instanceCreator = instanceCreator; 
            Debug.Log(_dataSave==null);
        }
        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  UniTask UpdateNoAdsStatus(bool advertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        protected abstract void UpdateLastSaveTime(string key);

        public void UpdateAllDataUI()
        {
            UpdateUINoAds(NoAdsStatus);
            UpdateUICountCoins(CountCoins);
        }
        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            ShopUI.UpdateViewNoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            ShopUI.UpdateCountCoins((int)DataSave[KeyData.COINS_COUNT]);
        }

    }
}
