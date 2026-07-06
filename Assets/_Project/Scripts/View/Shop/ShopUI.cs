using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
   public event Action OnPlayerClickBuyNoAds;
   public event Action OnPlayerClickBuy100Coins;

   private TMP_Text _textCoins;
   private Button _buttonBuyNoAds;
   private Button _buttonBuy100Coins;
   private Image _imageNoAds;
    public  void Initialize(Button buttonBuyNoAds, Button buttonBuy100Coins, TMP_Text textCoins, Image imageNoAds)
    {
        _textCoins = textCoins;
        _buttonBuy100Coins = buttonBuy100Coins;
        _buttonBuyNoAds = buttonBuyNoAds;
        _imageNoAds = imageNoAds;
        _buttonBuy100Coins.onClick.AddListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.AddListener(NotifyButtonTryBuyNoAds);
    }

    public void UpdateCountCoins(int endValue)
    {
        _textCoins.text = endValue.ToString();
    }

    private void OnDestroy()
    {
        _buttonBuy100Coins.onClick.RemoveListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.RemoveListener(NotifyButtonTryBuyNoAds);
    }
    public void UpdateViewNoAds(bool adsDisabled)
    {
        _imageNoAds.enabled = adsDisabled;
        _buttonBuyNoAds.interactable = !adsDisabled;
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

