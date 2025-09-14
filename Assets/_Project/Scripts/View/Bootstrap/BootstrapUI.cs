using Asteroid.Services.IAP;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.UI
{
    public class BootstrapUI : MonoBehaviour
    {
        public event Action OnPlayerClickButtonStart;
        public event Action OnPlayerClickBuyNoAds;

        [Inject(Id = "loadingSlider")] private Slider _sliderLoading;
        [Inject(Id = "buttonStart")] private Button _buttonStartGame;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        private void Start()
        {
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _buttonBuyNoAds.onClick.AddListener(NotifyButtonBuyNoAdsPressed);
        }

        public void UpdateSlider(float endValue)
        {
            endValue = Mathf.Clamp01(endValue);
            _sliderLoading.value = endValue;
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
    }
}
