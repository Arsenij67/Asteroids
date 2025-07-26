
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Asteroid.Generation
{
    public class LocalBundleSceneLoader : ISceneLoader
    {
        private const float MAX_LOADING_LEVEL = 1f;

        private string _lastSceneName = string.Empty;
        private AsyncOperationHandle<SceneInstance> _sceneLoadHandle;

        public float LoadingProgress => _sceneLoadHandle.IsValid() ? _sceneLoadHandle.PercentComplete : 0f;
        public string LastLoadedScene => _sceneLoadHandle.IsValid() && _sceneLoadHandle.PercentComplete >= MAX_LOADING_LEVEL ? _lastSceneName : string.Empty;

        public async void LoadScene(string name)
        {
            _lastSceneName = name;
            await LoadSceneAsync(name);
        }

        public void LoadSceneAdditive(string name)
        {
            _lastSceneName = name;
            LoadSceneAdditiveAsync(name).Forget();
        }

        public async UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
        {
            if (_lastSceneName.Equals(name))
            {
                _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive, allowSceneActivate);
                await _sceneLoadHandle.ToUniTask();
            }
            _lastSceneName = name;
        }

        public void ReloadScene(string nameId)
        {
            if (!_sceneLoadHandle.IsValid())
            {
                LoadScene(nameId);
            }
        }
        public UniTask SwitchSceneActivation(bool allowSceneBeActive)
        {
            var operationActivate = _sceneLoadHandle.Result.ActivateAsync();
            operationActivate.allowSceneActivation = allowSceneBeActive;
            return operationActivate.ToUniTask();
        }

        public void UnloadScene()
        {
            UnloadSceneAsync(_sceneLoadHandle).Forget();
        }

        public UniTask<object> ReloadSceneAsync(string name)
        {
            if (!_sceneLoadHandle.IsValid())
            {
                return LoadSceneAsync(name);
            }
            return UniTask.FromResult<object>(_sceneLoadHandle);
        }

        public async UniTask<object> LoadSceneAsync(string name)
        {
            if (_sceneLoadHandle.IsValid() && !(_lastSceneName.Equals(name)))
            {
                await UnloadSceneAsync(_sceneLoadHandle);
            }

            _lastSceneName = name;
            _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Single);
            await _sceneLoadHandle;
            return _sceneLoadHandle;
        }

        public async UniTask<object> UnloadSceneAsync(object data)
        {
            if (data == null)
            {
                Debug.LogWarning("Attempted to unload null scene");
            }

            else if (data is AsyncOperationHandle<SceneInstance> handle)
            {
                _sceneLoadHandle = handle;
                var handler = Addressables.UnloadSceneAsync(_sceneLoadHandle, autoReleaseHandle: false);
                await handler.ToUniTask();
                if (handler.Status == AsyncOperationStatus.Succeeded)
                {
                    _sceneLoadHandle = default;
                    Addressables.Release(handle);
                }
            }
            else
            {
                Debug.LogError($"Invalid type for scene unloading: {data.GetType()}");
            }
            return default;
        }
    }
}