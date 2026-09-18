using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Firebase;
using Firebase.RemoteConfig;
using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Asteroid.Services.RemoteConfig
{
    public class FirebaseRemoteConfigService : WIFIConnector, IRemoteConfigService, IDisposable
    {
        public event Action OnConfigUpdated;

        private bool _isInitialized = false;
        public bool IsInitialized => _isInitialized && IsConnected;

        public async UniTask Initialize(bool isServiceAvailable)
        {
            await IsConnectionAvailable();
            if (isServiceAvailable)
            {
                FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += HandleConfigUpdate;
                await FetchAndActivateAsync();
                _isInitialized = true;
                Debug.Log("Remote Config Initialized!");
            }
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

        public async UniTask FetchAndActivateAsync()
        {
            if (IsInitialized)
            {
                await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
                await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            }
        }

        public UniTask SetUserAttributes(string[] keys, string[] values)
        {
            if (!IsInitialized) return UniTask.CompletedTask;

                var userProperties = new Dictionary<string, object>();
            short indexValue = 0;
            foreach (var attr in keys)
            {
                userProperties[attr] = values[indexValue++];
            }
           return FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(userProperties).AsUniTask();
        }

        public void Dispose()
        {
            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= HandleConfigUpdate;
        }

        private void HandleConfigUpdate(object sender, ConfigUpdateEventArgs args)
        {
            if (args.Error != RemoteConfigError.None)
            {
                Debug.LogError($"Config update error: {args.Error}");
                return;
            }
            string updatedKeys = args.UpdatedKeys.Join(",");
            Debug.Log(string.Format($"Remote Config updated in realtime! Updated Keys : {updatedKeys}"));
            OnConfigUpdated?.Invoke();
        }
    }
}

