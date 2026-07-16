using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database
{
    public abstract class SaveStrategy
    {
        public bool NoAdsStatus => (bool)(DataForSave[KeyData.ADS_DISABLED] ?? false);
        public int CountCoins => (int)(DataForSave[KeyData.COINS_COUNT] ?? 0);

        protected ShopUI ShopUI;
        protected DataSave DataSave;
        protected DataSave DataForSave => DataSave ?? _instanceCreator.CreateInstance<DataSave>();

        private IInstanceLoader _instanceCreator;

        public void Initialize(DataSave dataSave, IInstanceLoader instanceCreator, ShopUI shopUI = null)
        {
            ShopUI = shopUI;
            DataSave = dataSave;
            _instanceCreator = instanceCreator; 
        }
        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  UniTask UpdateNoAdsStatus(bool advertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        protected abstract void UpdateLastSaveTime(string key);
        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            ShopUI.UpdateViewNoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            ShopUI.UpdateCountCoins((int)DataForSave[KeyData.COINS_COUNT]);
        }

    }
}
