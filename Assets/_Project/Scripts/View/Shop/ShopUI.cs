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
   public  void Initialize(Button buttonBuyNoAds, Button buttonBuy100Coins, TMP_Text textCoins)
    {
        _textCoins = textCoins;
        _buttonBuy100Coins = buttonBuy100Coins;
        _buttonBuyNoAds = buttonBuyNoAds;
        _buttonBuy100Coins.onClick.AddListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.AddListener(NotifyButtonTryBuyNoAds);
    }

    public void UpdateCountCoins(int endValue)
    {
        Debug.Log("UpdateCountCoins CALLED"+endValue);
        _textCoins.text = endValue.ToString();
    }

    private void OnDestroy()
    {
        _buttonBuy100Coins.onClick.RemoveListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.RemoveListener(NotifyButtonTryBuyNoAds);
    }

    private void NotifyButtonTryBuyNoAds()
    {
        OnPlayerClickBuyNoAds?.Invoke();
    }

    private void NotifyButtonTryAdd100CoinsBought()
    {
        OnPlayerClickBuy100Coins?.Invoke();
    }
}

