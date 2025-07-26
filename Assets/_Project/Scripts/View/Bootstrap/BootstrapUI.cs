using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace  Asteroid.UI
{ 
public class BootstrapUI : MonoBehaviour
{
    public event Action OnPlayerClickButtonStart;

    private Slider _sliderLoading;
    private Button _buttonStartGame;

    [Inject]
    public void Construct(Slider slider, Button button)
    {
        _sliderLoading = slider;
        _buttonStartGame = button;
        _buttonStartGame.onClick.AddListener(ActivateLoadedScene);
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

    private void ActivateLoadedScene()
    { 
        OnPlayerClickButtonStart?.Invoke();
    } 
}
}
