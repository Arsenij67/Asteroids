using UnityEngine;
using FirebaseWebGL;
using Cysharp.Threading.Tasks;
using Zenject;
using Asteroid.Database.Connection;
using System.Collections.Generic;

namespace Asteroid.Services.Analytics
{
    public class FirebaseAnalyticsSender : WIFIConnector,IAnalytics
    {
        private FirebaseApp _firebaseApplication;
        private bool _isInitialized = false;
        public bool AnalyticsEnabled => _isInitialized && IsConnected;

        public async UniTask<bool> Initialize (IServiceDependencyChecker application)
        {
#if !UNITY_WEBGL
            await IsConnectionAvailable();
#endif
            if (!application.IsAvailable)
            {
                return await UniTask.FromResult(false);
            }

            _firebaseApplication = application as FirebaseApp;
            _isInitialized = true;
            _firebaseApplication.Analytics.SetAnalyticsCollectionEnabled(true);
            return AnalyticsEnabled;
        }

        public void PushEvent<E>(string eventName, string parameterName, E parameterValue = default)
        {
            if (!AnalyticsEnabled)
            {
                Debug.LogWarning("Firebase Analytics not initialized. Event not sent.");
                return;
            }

            _firebaseApplication.Analytics.LogEvent
            (
                eventName, new Dictionary<string,object> () { { parameterName, parameterValue.ToString() } }
            );
        }

        public void PushUserProperty<P>(string propertyName, P propertyValue = default)
        {
            if (!AnalyticsEnabled)
            {
                Debug.LogWarning("Firebase Analytics not initialized. Property not set.");
                return;
            }

            _firebaseApplication.Analytics.SetUserProperties
            (
              new Dictionary<string, string>(){ { propertyName, propertyValue.ToString()}}
            );
        }
    }
}