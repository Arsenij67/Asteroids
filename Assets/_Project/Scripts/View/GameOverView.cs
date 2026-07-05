using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Asteroid.Generation;

public class GameOverView : MonoBehaviour
{
    public event Action OnGameReloadClicked;
    public event Action OnButtonShowAdsClicked;
    public event Action OnButtonGoHomeClicked;

    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonShowAd;
    [SerializeField] private Button _buttonGoHome;
    [SerializeField] private TMP_Text _enemiesDestroyedText;

    public void Initialize()
    {
        _buttonRestart.onClick.AddListener(OnRestartClicked);
        _buttonShowAd.onClick.AddListener(OnShowAdClicked);
        _buttonGoHome.onClick.AddListener(OnGoHomeClicked);
    }

    // ✅ Обычные методы для обработки нажатий
    private void OnRestartClicked()
    {
        OnGameReloadClicked?.Invoke();
    }

    private void OnShowAdClicked()
    {
        OnButtonShowAdsClicked?.Invoke();
    }

    private void OnGoHomeClicked()
    {
        OnButtonGoHomeClicked?.Invoke();
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void UpdateButtonShowAd(bool adsShowed)
    {
        _buttonShowAd.interactable = !adsShowed;
    }

    public void UpdateDestroyedEnemies(int count)
    {
        if (_enemiesDestroyedText != null)
        {
            _enemiesDestroyedText.text = $"Enemies destroyed: {count:D1} units";
        }
    }

    private void OnDestroy()
    {
        if (_buttonRestart != null)
        {  
            _buttonRestart.onClick.RemoveListener(OnRestartClicked);
        }

        if (_buttonShowAd != null)
        {
            _buttonShowAd.onClick.RemoveListener(OnShowAdClicked);
        }

        if (_buttonGoHome != null)
        {
            _buttonGoHome.onClick.RemoveListener(OnGoHomeClicked);
        }
    }
}