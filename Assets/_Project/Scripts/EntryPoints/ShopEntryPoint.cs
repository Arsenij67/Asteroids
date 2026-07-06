using Asteroid.Database;
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
        [Inject] private CloudDataPresenter _cloudDataPresenter;
        [Inject] private TMP_Text _textCoins;
        [Inject] private DataSave _dataSave;
        [Inject] private Image _imageNoAds;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

        private async void Start()
        {
            _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins, _imageNoAds);
            await _purchaseService.Initialize(_dataSave);
            _cloudDataPresenter.Initialize(_remoteSave, _dataSave,_shopUI);

            _purchaseService.OnPlayerBought100Coins += UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins += UpdateCoinsUI;
            _purchaseService.OnPlayerBoughtNoAds += UpdateNoAdsCloudAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds += UpdateNoAdsUI;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;

            _cloudDataPresenter.UpdateUINoAds();
            _cloudDataPresenter.UpdateUICountCoins();
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins -= UpdateCoinsUI;
            _purchaseService.OnPlayerBoughtNoAds -= UpdateNoAdsCloudAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds -= UpdateNoAdsUI;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        }

        private void UpdateNoAdsCloudAfterPurchase()
        {
            _cloudDataPresenter.UpdateNoAdsStatusCloud(_cloudDataPresenter.ADVERTISEMENT_IS_CANCELED);
        }

        private void UpdateNoAdsUI()
        {
           _cloudDataPresenter.UpdateUINoAds();
        }

        private async void UpdateCoinsAfterPurchase()
        {
            await _cloudDataPresenter.AddCountCoins(_cloudDataPresenter.ADDED_100_COINS);
        }

        private void UpdateCoinsUI()
        {
            _cloudDataPresenter.UpdateUICountCoins();
        }

    }
}
