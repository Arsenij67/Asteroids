using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
   public event Action OnPlayerClickBuyNoAds;
   public event Action OnPlayerClickBuy100Coins;

   [SerializeField] private TMP_Text _textCoins;
   [SerializeField] private Button _buttonBuy100Coins;
   [SerializeField] private Button _buttonBuyNoAds;
   [SerializeField] private Image _imageNoAds;

    private void OnDestroy()
    {
        _buttonBuy100Coins.onClick.RemoveListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.RemoveListener(NotifyButtonTryBuyNoAds);
    }

    public  void Initialize()
    {
        _buttonBuy100Coins.onClick.AddListener(NotifyButtonTryAdd100CoinsBought);
        _buttonBuyNoAds.onClick.AddListener(NotifyButtonTryBuyNoAds);
    }

    public void UpdateCountCoins(int endValue)
    {
        _textCoins.text = endValue.ToString();
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

