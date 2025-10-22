using Asteroid.Database;
using UnityEngine;
using UnityEngine.Advertisements;
using Unity;
using Zenject;

namespace Asteroid.Services.UnityAdvertisement
{
    public class UnityAdsAdvertisement:IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IAdvertisementService
    {
        private const string GAME_ANDROID_ID = "5916275";
        private const string GAME_IOS_ID = "5916274";

        private bool _isLoaded = false;
        private bool _isShowed = false;
        private DataSave _playerSave;

        private string ApplicationId => Application.platform == RuntimePlatform.IPhonePlayer ? GAME_IOS_ID : GAME_ANDROID_ID;

        public bool IsInitialized => Advertisement.isInitialized;

        public bool IsLoaded => _isLoaded;

        public bool IsShowed => _isShowed;

        public bool IsEnabled => !_playerSave.AdsDisabled;

        public void Initialize(params object[] parameters)
        {
           
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(ApplicationId, (bool)parameters[0], this);
            }
            _playerSave = (DataSave)parameters[1];
        }
        public void OnInitializationComplete()
        {
            Debug.Log("������������� ������ �������!");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("������������� ������ ����������!");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("������� ���������!");
            _isLoaded = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"������� {placementId} �� ���������! ��� ������� {error} {message}");
            _isLoaded = false;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"������� {placementId} �� ��������! ��� ������� {error} {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"������� {placementId} �������� ������������!" );
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"�� ������� {placementId} ������!");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"����� ������� {placementId} ��������! ��� ��������� �������: {showCompletionState}");
            Advertisement.Load(placementId, this);
            _isShowed = true;
        }

        public void Load(params object[] parameters)
        {
            Advertisement.Load(parameters[0].ToString() , this);
        }

        public void Show(params object[] parameters)
        {
            if (IsEnabled)
            {
                Advertisement.Show(parameters[0].ToString(), this);
            }
        }
    }
} 
