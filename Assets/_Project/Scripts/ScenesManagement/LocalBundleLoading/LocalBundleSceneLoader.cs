
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Asteroid.Generation
{
    public class LocalBundleSceneLoader : ISceneLoader
    {
        private Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();
        public async void LoadScene(string name)
        {
            var loadHandle = Addressables.LoadSceneAsync(name, activateOnLoad: true);
            _loadedScenes[name] = loadHandle;
            await loadHandle;
        }

        public void LoadSceneAdditive(string name)
        {
            LoadSceneAdditiveAsync(name).Forget();
        }

        public UniTask ReloadStartSceneAsync(string name)
        {
            if (_loadedScenes.ContainsKey(name) && _loadedScenes[name].IsValid())
            {
                 return UniTask.CompletedTask;
            }
            return LoadSceneAsync(name);
        }

        public async UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
        {
            if (!_loadedScenes.ContainsKey(name) || !_loadedScenes[name].IsValid())
            {
                _loadedScenes[name] = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive, allowSceneActivate);
                await _loadedScenes[name].ToUniTask();
            }
        }

        public void ReloadCurrentScene()
        {
            Scene sceneData = SceneManager.GetActiveScene();
            _loadedScenes[sceneData.name] =  default;
            LoadScene(sceneData.name);
        }
        public UniTask SwitchSceneActivation(string name, bool allowSceneBeActive)
        {
            if (_loadedScenes.ContainsKey(name))
            {
                var operationActivate = _loadedScenes[name].
                    Result.
                    ActivateAsync();
                operationActivate.allowSceneActivation = allowSceneBeActive;
                return operationActivate.ToUniTask();
            }
            return UniTask.CompletedTask;
        }

        public void UnloadScene(string name)
        {
            UnloadSceneAsync(name).Forget();
        }

        public UniTask ReloadSceneAsync(string name, bool activateOnLoad = true)
        {
            _loadedScenes[name] = Addressables.LoadSceneAsync(name, LoadSceneMode.Single, activateOnLoad);
            return _loadedScenes[name].ToUniTask();
        }

        public UniTask LoadSceneAsync(string name, bool activateOnLoad = true)
        {
            if ((_loadedScenes.ContainsKey(name) && _loadedScenes[name].IsValid()))
            {
                return UniTask.CompletedTask;

            }

            _loadedScenes[name] = Addressables.LoadSceneAsync(name, LoadSceneMode.Single, activateOnLoad);
           return _loadedScenes[name].ToUniTask();
        }

        public UniTask<object> UnloadSceneAsync(string name)
        {
            
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Scene name is null or empty");
                return default;
            }

            if (!_loadedScenes.ContainsKey(name))
            {
                Debug.LogWarning($"Scene '{name}' not found in loaded scenes dictionary");
                Debug.LogWarning($"Available scenes: {string.Join(", ", _loadedScenes.Keys)}");
                return default;
            }

            var sceneHandle = _loadedScenes[name];
            if (!sceneHandle.IsValid())
            {
                Debug.LogWarning($"Scene handle for '{name}' is invalid");
                _loadedScenes.Remove(name);
                return default;
            }

            var handler = Addressables.UnloadSceneAsync(sceneHandle, false);

            return handler.ToUniTask().ContinueWith((f) =>
            {
                if (handler.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedScenes.Remove(name);
                    Addressables.Release(handler);
                }
                else
                {
                    Debug.LogError($"Unload failed for scene: {name}, Status: {handler.Status}");
                }
                return default(object);
            });
    }   }
}