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
        public event Action OnPlayerClickButtonShop;

        [Inject(Id = "buttonToShop")] private Button _shopButton;
        [Inject(Id = "loadingSlider")] private Slider _sliderLoading;
        [Inject(Id = "buttonStart")] private Button _buttonStartGame;
        private void Start()
        {
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _shopButton.onClick.AddListener(NotifyShopButtonPressed);
        }
        private void NotifyShopButtonPressed()
        {
            Debug.Log("Нажали на магазин");
            OnPlayerClickButtonShop?.Invoke();
        }

        private void NotifyButtonStartPressed()
        {
            OnPlayerClickButtonStart?.Invoke();
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

    }
}
