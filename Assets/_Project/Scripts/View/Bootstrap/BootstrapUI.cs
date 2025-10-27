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
        public event Action OnPlayerClickButtonExit;

        [Inject(Id = "loadingSlider")] private Slider _sliderLoading;
        [Inject(Id = "buttonStart")] private Button _buttonStartGame;
        [Inject(Id = "buttonExit")] private Button _buttonExitGame;

        private void Start()
        {
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _buttonExitGame.onClick.AddListener(NotifyButtonExitPressed);
        }

        private void OnDestroy()
        {
            _buttonStartGame.onClick.RemoveListener(NotifyButtonStartPressed);
            _buttonExitGame.onClick.RemoveListener(NotifyButtonExitPressed);
        }

        private void NotifyButtonStartPressed()
        {
            OnPlayerClickButtonStart?.Invoke();
        }

        private void NotifyButtonExitPressed()
        {
            OnPlayerClickButtonExit?.Invoke();
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
