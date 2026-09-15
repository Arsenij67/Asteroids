using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    public event Action OnGameReloadClicked;
    public event Action OnButtonShowAdsClicked;
    public event Action OnButtonGoHomeClicked;

    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonShowAd;
    [SerializeField] private Button _buttonGoHome;
    [SerializeField] private TMP_Text _enemiesDestroyedText;

    [Header("Tuning")]
    [SerializeField] private float _animationDuration = 1.0f;
    [SerializeField] private float _shareIncreaseWindow = 0.15f;
    [SerializeField] private int _countFadeAway = 4;

    private Sequence _sequence;
    private RectTransform _window;

    public async void Initialize()
    {
        _buttonRestart.onClick.AddListener(OnRestartClicked);
        _buttonShowAd.onClick.AddListener(OnShowAdClicked);
        _buttonGoHome.onClick.AddListener(OnGoHomeClicked);
        _window = GetComponent<RectTransform>();
        await PlayShow();
    }

    public UniTask PlayShow()
    {
        _sequence?.Kill();
        _window = transform.GetComponent<RectTransform>();
        _sequence = DOTween.Sequence()
                .Append(_window.DOScale(Vector2.zero, 0)
                .SetEase(Ease.OutCubic))
                .Append(_window.DOScale(Vector3.one, _animationDuration))
                .Append(_window.DOPunchScale(Vector3.one * _shareIncreaseWindow, _animationDuration, _countFadeAway))
                .SetUpdate(true)
                .SetLink(gameObject); 
        return _sequence.AsyncWaitForCompletion().AsUniTask();  
    }

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
        if (this.gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateButtonShowAd(bool adsShowed, bool adsInitialized)
    {
        _buttonShowAd.interactable = !adsShowed && adsInitialized;
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
        _sequence?.Kill();

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