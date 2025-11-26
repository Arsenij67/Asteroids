using Asteroid.Database;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using TMPro;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopEntryPoint : MonoBehaviour
{
    private const int ADDED_100_COINS = 100;

    [Inject] private ShopUI _shopUI;
    [Inject] private IPurchasingService _purchaseService;
    [Inject] private IRemoteSavable _remoteSave;
    [Inject] private CloudDataController _cloudDataController;
    [Inject] private TMP_Text _textCoins;
    [Inject] private DataSave _dataSave;
    [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
    [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

    private void Start()
    {
        _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins);
        _purchaseService.Initialize(_shopUI);
        _cloudDataController.Initialize(_remoteSave);
        _shopUI.UpdateCountCoins(_dataSave.CountCoins);
        _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
        _shopUI.OnPlayerClickBuy100Coins +=  () =>_cloudDataController.AddCountCoins(ADDED_100_COINS);
    }

    private void OnDestroy()
    {
        _shopUI.OnPlayerClickBuy100Coins -= () => _cloudDataController.AddCountCoins(ADDED_100_COINS);
        _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
    }
}
