
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YG;

namespace Asteroid.Services.IAP
{
    public class IAPReceiverYandexGames : IPurchasingService
    {
        private const string NO_ADS_ID = "NO_ADS";
        private const string COINS_100_ID = "COINS_100";

        public bool IsInitialized { get; private set; }

        public event Func<int, UniTask> OnPlayerBoughtCoins;
        public event Func<bool, UniTask> OnPlayerBoughtNoAds;

        private readonly int _added100Coins = 100;
        private readonly bool _advertisementIsCanceled = true;

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => YG2.isSDKEnabled);
            YG2.ConsumePurchases();
            SubscribeListeners();
            YG2.SyncInitialization();
        }

        private void SuccessPurchased(string id)
        {
            if (id.Equals(COINS_100_ID))
            {
                OnPlayerBoughtCoins?.Invoke(_added100Coins);
            }
            else if (id.Equals(NO_ADS_ID))
            {
                OnPlayerBoughtNoAds?.Invoke(_advertisementIsCanceled);
            }
            Debug.Log($"Order: {id}, Status: Succeeded ");
        }

        private void FailedPurchased(string id)
        {
            Debug.Log("FAILED");
 
        }
        public void Buy100Coins()
        {
            BuyProduct(COINS_100_ID);
        }

        public void BuyNoAds()
        {
            BuyProduct(NO_ADS_ID);
        }

        public void Dispose()
        {
            UnsubscribeListeners();
            IsInitialized = false;
        }

        private void BuyProduct(string productId)
        {
            YG2.BuyPayments(productId);
        }

        private void SubscribeListeners()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;
            YG2.onPurchaseFailed += FailedPurchased;
            YG2.onGetPayments += UpdatedPurchases;
        }

        private void UpdatedPurchases()
        {
            Debug.Log("Была попытка купить");
        }

        private void UnsubscribeListeners()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
            YG2.onPurchaseFailed -= FailedPurchased;
            YG2.onGetPayments -= UpdatedPurchases;
        }
    }
}
