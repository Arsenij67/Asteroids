using UnityEngine;
using Firebase;
using Firebase.Analytics;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Asteroid.Services
{
    public class FirebaseAnalyticsSender : IAnalytics
    {
        private bool _isInitialized = false;
        public bool AnalyticsEnabled => _isInitialized;

        public async UniTask<bool> Initialize()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
          
            if (dependencyStatus == DependencyStatus.Available)
            {
                _isInitialized = true;
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase Analytics initialized successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
            return _isInitialized;
        }

        public void PushEvent<E>(string eventName, string parameterName, E parameterValue = default)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Firebase Analytics not initialized. Event not sent.");
                return;
            }

            Firebase.Analytics.FirebaseAnalytics.LogEvent(
                eventName,
                parameterName,
                parameterValue.ToString()
            );
        }

        public void PushUserProperty<P>(string propertyName, P propertyValue = default)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Firebase Analytics not initialized. Property not set.");
                return;
            }

            Firebase.Analytics.FirebaseAnalytics.SetUserProperty(
                propertyName,
                propertyValue.ToString()
            );
        }

        public void ResetAnalyticsData()
        {
            if (!_isInitialized) return;

            Firebase.Analytics.FirebaseAnalytics.ResetAnalyticsData();
            Debug.Log("Firebase Analytics data reset");
        }
    }
}