using Asteroid.Statistic;
using System;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Button  _buttonGoHome;
    [SerializeField] private TMP_Text _enemiesDestroyedText;

    private IResourceLoaderService _resourceLoaderService;
    private Transform _placeMounting;
    public void Initialize(IResourceLoaderService resourceLoaderService, Transform placeMounting)
    {
        _buttonRestart.onClick.AddListener(() => { OnGameReloadClicked.Invoke(); });
        _buttonShowAd.onClick.AddListener(() =>  { OnButtonShowAdsClicked.Invoke(); });
        _buttonGoHome.onClick.AddListener(() =>  { OnButtonGoHomeClicked.Invoke(); });

        _placeMounting  = placeMounting;
        _resourceLoaderService = resourceLoaderService;
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
        _buttonRestart.onClick.RemoveListener(() => { OnGameReloadClicked.Invoke(); });
        _buttonShowAd.onClick.RemoveListener(() => { OnButtonShowAdsClicked.Invoke(); });
        _buttonGoHome.onClick.RemoveListener(() => { OnButtonGoHomeClicked.Invoke(); });
    }

}
