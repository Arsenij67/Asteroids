
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

    private SceneInstance _currentScene;
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
                _currentScene = await _sceneLoadHandle.ToUniTask();
            }
            _lastSceneName = name;
     }

    public void ReloadScene(string nameId)
    {
            Debug.Log("scene load handle: " + _sceneLoadHandle.IsValid()+" "+ _sceneLoadHandle.GetHashCode());
        if (!_sceneLoadHandle.IsValid())
        {
            Debug.Log($"Scene {nameId} reloaded");
            LoadScene(nameId);
        }
    }
        public void SwitchSceneActivation(bool allowSceneBeActive)
        {

            {

                var op = _sceneLoadHandle
                       .Result
                       .ActivateAsync();
                op.allowSceneActivation = allowSceneBeActive;

         

            }

        }

        public UniTask SwitchSceneActivation(bool allowSceneBeActive,int i =0)
    {
       
        {

                var op = _sceneLoadHandle
                       .Result
                       .ActivateAsync();
                op.allowSceneActivation = allowSceneBeActive;

                return op.ToUniTask();
 
        }

    }

    private bool IsSceneReady()
    {
        return _sceneLoadHandle.IsValid()
               && _sceneLoadHandle.IsDone
               && _sceneLoadHandle.Result.Scene.isLoaded;
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
            Debug.Log("Start...");
            if (data == null)
            {
                Debug.LogWarning("Attempted to unload null scene");
                return default;
            }

            else if (data is AsyncOperationHandle<SceneInstance> handle)
            {
                Debug.Log("Start..."+ handle.Result.Scene.name);
                _sceneLoadHandle = handle;
                var handler = Addressables.UnloadSceneAsync(_sceneLoadHandle, autoReleaseHandle: false);
                await handler.ToUniTask();
           
                if (handler.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentScene = default;
                    _sceneLoadHandle = default;
                    Addressables.Release(handle);
                }
            }
            else
            {
                Debug.LogError($"Invalid type for scene unloading: {data.GetType()}");
                return default;
            }

            
            return default;
        }
    }
}