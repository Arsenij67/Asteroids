using Asteroid.Services.IAP;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.UI
{
    public class BootstrapUI : MonoBehaviour
    {
        public event Action OnPlayerClickButtonStart;
        public event Action OnPlayerClickBuyNoAds;
        public event Action OnPlayerClickBuy100Coins;

        [Inject] private TMP_Text _textCoins;
        [Inject(Id = "loadingSlider")] private Slider _sliderLoading;
        [Inject(Id = "buttonStart")] private Button _buttonStartGame;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;
        private void Start()
        {
            _buttonBuy100Coins.onClick.AddListener(NotifyButtonAdd100CoinsPressed);
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _buttonBuyNoAds.onClick.AddListener(NotifyButtonBuyNoAdsPressed);
        }

        public void UpdateSlider(float endValue)
        {
            endValue = Mathf.Clamp01(endValue);
            _sliderLoading.value = endValue;
        }

        public void UpdaaeCountCoins(int endValue)
        { 
          _textCoins.text = endValue.ToString();      
        }

        public void ActivateButtonStart()
        {
            if (_buttonStartGame != null)
            {
                _buttonStartGame.gameObject.SetActive(true);
            }
        }

        private void NotifyButtonStartPressed()
        {
            OnPlayerClickButtonStart?.Invoke();
        }

        private void NotifyButtonBuyNoAdsPressed()
        {
            OnPlayerClickBuyNoAds?.Invoke();
        }

        private void NotifyButtonAdd100CoinsPressed()
        { 
            OnPlayerClickBuy100Coins?.Invoke();
        }
    }
}
