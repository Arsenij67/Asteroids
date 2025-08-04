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

        private bool _isLoaded = false;

        public bool isInitialized => Advertisement.isInitialized;

        public bool isLoaded => _isLoaded;

        public bool isShowing => Advertisement.isShowing;

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
        }

        public void Initialize(params object[] parameters)
        {
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(GAME_ANDROID_ID, (bool)parameters[0], this);
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
