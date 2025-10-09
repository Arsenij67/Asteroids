using Asteroid.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopEntryPoint : MonoBehaviour
{
    [Inject] ShopUI _shopUI;
    [Inject] IPurchasingService _purchaseService;
    [Inject] private TMP_Text _textCoins;
    [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
    [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;
    private void Start()
    {
        _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins);
        _purchaseService.Initialize(_shopUI);
        _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
    }

    private void OnDestroy()
    {
        _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
    }
}
