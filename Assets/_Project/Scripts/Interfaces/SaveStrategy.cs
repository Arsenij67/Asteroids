using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database
{
    public abstract class SaveStrategy
    {
        public bool NoAdsStatus => (bool)(DataSave[KeyData.ADS_DISABLED] ?? false);
        public int CountCoins => (int)(DataSave[KeyData.COINS_COUNT] ?? 0);

        protected ShopUI ShopUI;
        protected DataSave DataSave;

        public void Initialize(DataSave dataSave, ShopUI shopUI = null)
        {
            ShopUI = shopUI;
            DataSave = dataSave;
        }
        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  UniTask UpdateNoAdsStatus(bool adevertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        protected abstract void UpdateLastSaveTime(string key);

        public DataSave GetDataSave()
        { 
            return DataSave ?? new DataSave();
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
