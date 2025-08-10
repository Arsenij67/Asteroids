using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Firebase;
using Firebase.RemoteConfig;
using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroid.Services.RemoteConfig
{
    public class FirebaseRemoteConfigService : IRemoteConfigService, IDisposable, ITickable
    {
        public event Action OnConfigUpdated;

        private bool _isInitialized = false;
        public bool IsInitialized => _isInitialized;

        public async UniTask Initialize()
        {
            DependencyStatus statusResult =  await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (statusResult == DependencyStatus.Available)
            {
                FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += HandleConfigUpdate;
                await FetchAndActivateAsync();
                _isInitialized = true;
                Debug.Log("Remote Config Initialized!");
            }
            else
            {
                Debug.LogError($"Firebase init failed: {statusResult}");
            }
        }

        private void HandleConfigUpdate(object sender, ConfigUpdateEventArgs args)
        {
            if (args.Error != null)
            {
                Debug.LogError($"Config update error: {args.Error}");
                return;
            }
            string updatedKeys = args.UpdatedKeys.Join(",");
            Debug.Log(string.Format($"Remote Config updated in realtime! Updated Keys : {updatedKeys}"));
            OnConfigUpdated?.Invoke();
        }

        public T GetValue<T>(string key)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Remote Config not initialized. Returning default value.");
                return default;
            }
            ConfigValue configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
            return (T)Convert.ChangeType(configValue.StringValue, typeof(T));
        }

        public UniTask FetchAndActivateAsync()
        {
           return FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().AsUniTask();   
        }

        public UniTask SetUserAttributes(string[] attributes)
        {
            var userProperties = new Dictionary<string, object>();
            foreach (var attr in attributes)
            {
                userProperties[attr] = true;
            }
           return FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(userProperties).AsUniTask();
        }

        public void Dispose()
        {
            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= HandleConfigUpdate;
        }

        public void Tick()
        {
            Debug.Log("RC - "+ _isInitialized);
        }
    }
}

