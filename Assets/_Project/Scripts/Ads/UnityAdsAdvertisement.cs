using System;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Asteroid.UnityAdvertisement
{
    public class UnityAdsAdvertisement: IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IAdvertisementService
    {
        private const string GAME_ANDROID_ID = "5916275";
        private const string GAME_IOS_ID = "5916274";

        private bool _isLoaded = false;
        private bool _isShowed = false;

        private string ApplicationId => Application.platform == RuntimePlatform.IPhonePlayer ? GAME_IOS_ID : GAME_ANDROID_ID;

        public bool isInitialized => Advertisement.isInitialized;

        public bool isLoaded => _isLoaded;

        public bool isShowed => _isShowed;

        public void OnInitializationComplete()
        {
            Debug.Log("Инициализайия прошла успешно!");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("Инициализайия прошла безуспешно!");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Реклама загружена!");
            _isLoaded = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Реклама {placementId} не загружена! Вот причина {error} {message}");
            _isLoaded = false;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Реклама {placementId} не показана! Вот причина {error} {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Реклама {placementId} началась показываться!" );
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"На рекламу {placementId} нажали!");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"Показ рекламы {placementId} закончен! Вот состояние рекламы: {showCompletionState}");
            Advertisement.Load(placementId, this);
            _isShowed = true;
        }

        public void Initialize(params object[] parameters)
        {
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(ApplicationId, (bool)parameters[0], this);
            }
        }

        public void Load(params object[] parameters)
        {
            Advertisement.Load(parameters[0].ToString() , this);
        }

        public void Show(params object[] parameters)
        {
            Advertisement.Show(parameters[0].ToString(), this);
        }
    }
}
