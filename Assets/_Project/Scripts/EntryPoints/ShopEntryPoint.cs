using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.EntryPoints
{
    public class ShopEntryPoint : MonoBehaviour
    {
        [Inject] private ShopUI _shopUI;
        [Inject] private IPurchasingService _purchaseService;
        [Inject] private IRemoteSavable _remoteSave;
        [Inject] private SaveStrategy _currentSaveStrategy;
        [Inject] private TMP_Text _textCoins;
        [Inject] private DataSave _dataSave;
        [Inject] private Image _imageNoAds;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

        private async void Start()
        {
            _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins, _imageNoAds);
            await _purchaseService.Initialize(_dataSave);
            _currentSaveStrategy.Initialize(_dataSave,_shopUI, _remoteSave);

            _purchaseService.OnPlayerBought100Coins += UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins += _currentSaveStrategy.UpdateUICountCoins; 
            _purchaseService.OnPlayerBoughtNoAds += _currentSaveStrategy.UpdateNoAdsStatus;
            _purchaseService.OnPlayerBoughtNoAds += _currentSaveStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;

            _shopUI.UpdateViewNoAds(_currentSaveStrategy.NoAdsStatus);
            _shopUI.UpdateCountCoins(_currentSaveStrategy.CountCoins);
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins -= _currentSaveStrategy.UpdateUICountCoins;
            _purchaseService.OnPlayerBoughtNoAds -= _currentSaveStrategy.UpdateNoAdsStatus;
            _purchaseService.OnPlayerBoughtNoAds -= _currentSaveStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        }

        private async void UpdateCoinsAfterPurchase(int countCoins)
        {
            await _currentSaveStrategy.AddCountCoins(countCoins);
        }

    }
}
