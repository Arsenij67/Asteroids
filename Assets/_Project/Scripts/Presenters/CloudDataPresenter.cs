
using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataPresenter
    {
        public readonly int ADDED_100_COINS = 100;
        public readonly bool ADVERTISEMENT_IS_CANCELED = true;
        public int CountCoins=> _dataSave[CloudKeyData.COINS_COUNT]!=null ? (int)_dataSave[CloudKeyData.COINS_COUNT] : 0;
        public bool NoAdsStatus => (bool)_dataSave[CloudKeyData.ADS_DISABLED];

        private IRemoteSavable _remoteSavable;
        private DataSave _dataSave;
        private ShopUI _shopUI; 
        public void Initialize(IRemoteSavable remoteSavable, DataSave dataSave, ShopUI shopUI=null)
        { 
            _remoteSavable = remoteSavable;   
            _dataSave = dataSave;
            _shopUI= shopUI;
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
        public void UpdateNoAdsStatusCloud()
        {
            _dataSave[CloudKeyData.ADS_DISABLED] = NoAdsStatus;
            _remoteSavable.SaveKey(CloudKeyData.ADS_DISABLED, _dataSave[CloudKeyData.ADS_DISABLED]);
        }
        public async void RemoveCountCoins(int coinsToRemove)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(CloudKeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(CloudKeyData.COINS_COUNT, oldCoins - coinsToRemove);
        }

        public void UpdateUINoAds()
        {
            _shopUI.UpdateViewNoAds(NoAdsStatus);  
        }

        public void UpdateUICountCoins()
        {
            _shopUI.UpdateCountCoins(CountCoins);
        }
    }
}
