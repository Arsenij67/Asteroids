
using Cysharp.Threading.Tasks;
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
            _loadedScenes.Add(name,loadHandle);
            await loadHandle;
        }

        public void LoadSceneAdditive(string name)
        {
             
            LoadSceneAdditiveAsync(name).Forget();
        }

        public async UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
        {
            if (!_loadedScenes.ContainsKey(name))
            { 

                _loadedScenes[name] = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive, allowSceneActivate);
                await _loadedScenes[name].ToUniTask();
                Debug.Log("—цена загружена!" + name);
            }
        }

        public void ReloadCurrentScene()
        {
            Scene sceneData = SceneManager.GetActiveScene();
            _loadedScenes.Add(sceneData.name,default);
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

        public UniTask<object> ReloadSceneAsync(string name)
        {
            return LoadSceneAsync(name);   
        }

        public async UniTask<object> LoadSceneAsync(string name)
        {
            if ((_loadedScenes.ContainsKey(name)&&_loadedScenes[name].IsValid()))
            {
                 
                await UnloadSceneAsync(name);
                _loadedScenes.Remove(name);
                return UniTask.CompletedTask;
            }

            Debug.Log("—цена загружена!" + name);

            _loadedScenes[name]= Addressables.LoadSceneAsync(name, LoadSceneMode.Single);
            await _loadedScenes[name];
            return _loadedScenes[name];
        }

        public async UniTask<object> UnloadSceneAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || !_loadedScenes.ContainsKey(name))
            {
                Debug.LogWarning("Attempted to unload null scene");
            }

            else
            {
                var handler = Addressables.UnloadSceneAsync(_loadedScenes[name], autoReleaseHandle: false);
                await handler.ToUniTask();
                if (handler.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedScenes.Remove(name);
                    Addressables.Release(handler);
                }
            }
            return default;
        }
    }
}