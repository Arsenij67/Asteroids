using Asteroid.Database;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using Asteroid.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopEntryPoint : MonoBehaviour
{
    private const int ADDED_100_COINS = 100;
    private const bool  ADVERTISEMENT_IS_CANCELED = true;

    [Inject] private ShopUI _shopUI;
    [Inject] private IPurchasingService _purchaseService;
    [Inject] private IRemoteSavable _remoteSave;
    [Inject] private CloudDataController _cloudDataController;
    [Inject] private TMP_Text _textCoins;
    [Inject] private DataSave _dataSave;
    [Inject] private Image _imageNoAds;
    [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
    [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

    private async void Start()
    {
        _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins,_imageNoAds);
        await _purchaseService.Initialize(_dataSave);
        _cloudDataController.Initialize(_remoteSave,_dataSave);

        _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
        _purchaseService.OnPlayerBought100Coins += async () => await _cloudDataController.AddCountCoins(ADDED_100_COINS);
        _purchaseService.OnPlayerBought100Coins += () => _shopUI.UpdateCountCoins(_cloudDataController.CountCoins+ADDED_100_COINS);
        _purchaseService.OnPlayerBoughtNoAds += () => _cloudDataController.UpdateNoAdsStatus(ADVERTISEMENT_IS_CANCELED);
        _purchaseService.OnPlayerBoughtNoAds += () => _shopUI.UpdateViewNoAds(ADVERTISEMENT_IS_CANCELED);

        _shopUI.UpdateViewNoAds(_cloudDataController.NoAdsStatus);
        _shopUI.UpdateCountCoins(_cloudDataController.CountCoins);
    }

    private void OnDestroy()
    {
        _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
        _shopUI.OnPlayerClickBuy100Coins -= () => _shopUI.UpdateCountCoins((int)_dataSave[CloudKeyData.COINS_COUNT]);
        _purchaseService.OnPlayerBought100Coins -= async () => await _cloudDataController.AddCountCoins(ADDED_100_COINS);
    }
}
