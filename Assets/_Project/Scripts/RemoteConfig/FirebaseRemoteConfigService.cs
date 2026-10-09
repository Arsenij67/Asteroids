using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Firebase.RemoteConfig;
using FirebaseWebGL;
using ModestTree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace Asteroid.Services.RemoteConfig
{
    public class FirebaseRemoteConfigService : WIFIConnector, IRemoteConfigService, IDisposable
    {
        public event Action OnConfigUpdated;

        private bool _isInitialized = false;
        private FirebaseApp _firebaseApplication;

        public bool IsInitialized => _isInitialized && IsConnected;

        public async UniTask Initialize(IServiceDependencyChecker application)
        {
            if (!application.IsAvailable)
            {
                await  UniTask.CompletedTask;
            }
            _firebaseApplication = application as FirebaseApp;

            await IsConnectionAvailable();
            _firebaseApplication.RemoteConfig.OnConfigUpdate(HandleConfigUpdate);
            _isInitialized = true;
            await FetchAndActivateAsync();

            ConfigValue configValue = JsonConvert.DeserializeObject<ConfigValue>(_firebaseApplication.RemoteConfig.GetString("ship_config"));
        }

        public T GetValue<T>(string key)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Remote Config not initialized. Returning default value.");
                return default;
            }
            ConfigValue configValue = JsonConvert.DeserializeObject <ConfigValue> (_firebaseApplication.RemoteConfig.GetString(key));
            return (T)Convert.ChangeType(configValue.StringValue, typeof(T));
        }

        public async UniTask FetchAndActivateAsync()
        {
            if (IsInitialized)
            {
                _firebaseApplication.RemoteConfig.FetchConfig((firebaseCallback=>

                {
                    if (!firebaseCallback.success)
                    {
                        Debug.LogError($"Fetch: {firebaseCallback.error}");
                        return;
                    }
                    Debug.Log($"Fetch: {firebaseCallback.result}");
                }
                
                ));
                _firebaseApplication.RemoteConfig.Activate((firebaseCallback =>

                {
                    if (!firebaseCallback.success)
                    {
                        Debug.LogError($"Activate: {firebaseCallback.error}");
                        return;
                    }
                    Debug.Log($"Activate: {firebaseCallback.result}");
                }
                ));
            }
        }

        public void SetUserAttributes(string[] keys, string[] values)
        {
            short indexValue = 0;
            if (!IsInitialized) return;

            var userDictionary = new Dictionary<string, string>();
            foreach (var attr in keys)
            {
                userDictionary[attr] = values[indexValue++];
            }
            var userProperties = new ReadOnlyDictionary<string, string>(userDictionary);
            _firebaseApplication.RemoteConfig.SetCustomSignals(userProperties);
        }

        private void HandleConfigUpdate(string[] updatedKeys)
        {
            Debug.Log(string.Format($"Remote Config updated in realtime! Updated Keys : {updatedKeys.Join(",")}"));
            OnConfigUpdated?.Invoke();
        }
    }
}

