using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.EntryPoints
{
    public class ShopEntryPoint : MonoBehaviour
    {
        [Inject] private ShopUI _shopUI;
        [Inject] private IPurchasingService _purchaseService;
        [Inject] private IInstanceLoader _instanceLoader;
        [Inject] private LocalSaveStrategyPresenter _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private SaveDataStrategyController _saveDataStrategy;
        [Inject] private TMP_Text _textCoins;
        [Inject] private DataSave _dataSave;
        [Inject] private LocalSaveData _localSave;
        [Inject] private Image _imageNoAds;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

        private async void Start()
        {
            _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins, _imageNoAds);
            await _purchaseService.Initialize(_dataSave);
            await _localSaveStrategy.Initialize(_dataSave,_localSave, _instanceLoader,_shopUI);
            await _saveDataStrategy.Initialize(_cloudSaveStrategy, _localSaveStrategy);

            _purchaseService.OnPlayerBought100Coins += UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins += _localSaveStrategy.UpdateUICountCoins; 
            _purchaseService.OnPlayerBoughtNoAds += UpdateNoAdsAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds += _localSaveStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;

            _shopUI.UpdateViewNoAds(_localSaveStrategy.NoAdsStatus);
            _shopUI.UpdateCountCoins(_localSaveStrategy.CountCoins);
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins -= _localSaveStrategy.UpdateUICountCoins;
            _purchaseService.OnPlayerBoughtNoAds -= UpdateNoAdsAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds -= _localSaveStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        }

        private async void UpdateNoAdsAfterPurchase(bool isCanceled)
        {
            await _localSaveStrategy.UpdateNoAdsStatus(isCanceled);
        }
        private async void UpdateCoinsAfterPurchase(int countCoins)
        {
            await _localSaveStrategy.AddCountCoins(countCoins);
        }

    }
}
