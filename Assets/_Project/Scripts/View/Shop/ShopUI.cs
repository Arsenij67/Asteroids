using Asteroid.Services.IAP;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopUI : MonoBehaviour
{
   public event Action OnPlayerClickBuyNoAds;
   public event Action OnPlayerClickBuy100Coins;

   private TMP_Text _textCoins;
   private Button _buttonBuyNoAds;
   private Button _buttonBuy100Coins;
   private IPurchasingService _purchasingService;

   public  void Initialize(Button buttonBuyNoAds, Button buttonBuy100Coins, TMP_Text textCoins)
    {
        _textCoins = textCoins;
        _buttonBuy100Coins = buttonBuy100Coins;
        _buttonBuyNoAds = buttonBuyNoAds;
        _buttonBuy100Coins.onClick.AddListener(NotifyButtonAdd100CoinsPressed);
        _buttonBuyNoAds.onClick.AddListener(NotifyButtonBuyNoAdsPressed);
    }

    public void UpdateCountCoins(int endValue)
    {
        _textCoins.text = endValue.ToString();
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        _buttonBuy100Coins.onClick.RemoveListener(NotifyButtonAdd100CoinsPressed);
        _buttonBuyNoAds.onClick.RemoveListener(NotifyButtonBuyNoAdsPressed);
    }
    private void NotifyButtonBuyNoAdsPressed()
    {
        OnPlayerClickBuyNoAds?.Invoke();
    }

    private void NotifyButtonAdd100CoinsPressed()
    {
        Debug.Log("Update");
        OnPlayerClickBuy100Coins?.Invoke();
    }
}

