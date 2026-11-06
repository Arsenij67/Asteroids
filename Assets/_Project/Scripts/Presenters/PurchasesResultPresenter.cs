using Asteroid.Generation;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Asteroid.Services.IAP
{
    public class PurchasesResultPresenter
    {
        private Transform _parentToPin;
        private PurchasesResultData _shopNotificationData;
        private PurchasesResultView _purchasesResultView;
        private IResourceLoader _resourceLoader;

        public void Initialize(PurchasesResultData shopNotificationData,Transform parentToPin, IResourceLoader resourceLoader)
        { 
            _shopNotificationData = shopNotificationData;
            _resourceLoader = resourceLoader;
            _parentToPin = parentToPin;
        }

        public async UniTask OpenResultPurchasedCoins(int addCoins)
        {
            _purchasesResultView =  await _resourceLoader.InstantiateAsync(_shopNotificationData.WindowPrefab, _parentToPin);
            _purchasesResultView.Initialize(_resourceLoader);
            await _purchasesResultView.OpenWindow(Regex.Replace(_shopNotificationData.TextForCoins,@"\d+",addCoins.ToString()), _shopNotificationData.CoinSprite);
        }

        public async UniTask OpenResultPurchasedAds(bool isActive)
        {
            _purchasesResultView = await _resourceLoader.InstantiateAsync(_shopNotificationData.WindowPrefab, _parentToPin);
            _purchasesResultView.Initialize(_resourceLoader);
            await _purchasesResultView.OpenWindow(_shopNotificationData.TextForNoAds, _shopNotificationData.NoAdsSprite);
        }

    }
}
