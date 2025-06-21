using System;
using UnityEngine;
using UnityEngine.UI;
public class BootstrapUI : MonoBehaviour
{
    public event Action OnPlayerClickButtonStart;

    [SerializeField] private Slider _slider;
    [SerializeField] private Button _buttonStartGame;

    private void Start()
    {
        _buttonStartGame.onClick.AddListener(ActivateLoadedScene);
    }
    public void UpdateSlider(float endValue)
    { 
        endValue = Mathf.Clamp01(endValue);
        _slider.value = endValue;
    }

    public void ActivateButtonStart()
    {
        _buttonStartGame.gameObject.SetActive(true);
    }

    private void ActivateLoadedScene()
    { 
        OnPlayerClickButtonStart?.Invoke();
    }
    

}
