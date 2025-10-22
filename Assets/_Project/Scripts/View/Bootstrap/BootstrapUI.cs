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

        [Inject(Id = "loadingSlider")] private Slider _sliderLoading;
        [Inject(Id = "buttonStart")] private Button _buttonStartGame;
        private void Start()
        {
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
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
