using System;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroid.UI
{
    public class BootstrapUI : MonoBehaviour, IDisposable
    {
        public event Action OnPlayerClickButtonStart;
        public event Action OnPlayerClickButtonExit;

        [SerializeField] private Slider _sliderLoading;
        [SerializeField] private Button _buttonStartGame;
        [SerializeField] private Button _buttonExitGame;
        [SerializeField] private RectTransform _interfaceMount;

        public void Initialize()
        {
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _buttonExitGame.onClick.AddListener(NotifyButtonExitPressed);
        }

        public void SetUpUI(RectTransform parent)
        {
            _interfaceMount.SetParent(parent, false);
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

        public void Dispose()
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
    }
}
