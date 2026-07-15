using UnityEngine;
using Firebase;
using Firebase.Analytics;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Zenject;
using Asteroid.Database.Connection;

namespace Asteroid.Services.Analytics
{
    public class FirebaseAnalyticsSender : Connector,IAnalytics
    {
        private bool _isInitialized = false;
        public bool AnalyticsEnabled => _isInitialized && IsConnected;

        public async UniTask<bool> Initialize()
        {
            await IsConnectionAvailable();
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
          
            if (dependencyStatus == DependencyStatus.Available)
            {
                _isInitialized = true;
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
            return AnalyticsEnabled;
        }

        public void PushEvent<E>(string eventName, string parameterName, E parameterValue = default)
        {
            if (!AnalyticsEnabled)
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
            if (!AnalyticsEnabled)
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
            if (!AnalyticsEnabled) return;

            Firebase.Analytics.FirebaseAnalytics.ResetAnalyticsData();
            Debug.Log("Firebase Analytics data reset");
        }
    }
}