
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

    public async UniTask LoadSceneAsync(string name)
    {
        if (_sceneLoadHandle.IsValid())
        {
            await UnloadSceneAsync();
        }
        _lastSceneName = name;
        _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Single);
        _currentScene = await _sceneLoadHandle.ToUniTask();
    }

    public async UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
    {
            if (_sceneLoadHandle.IsValid() && _lastSceneName.Equals(name))
            {
                await UnloadSceneAsync();
            }

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

    public async void SwitchSceneActivation(bool allowSceneBeActive)
    {
        if (true)
        {

                var op = _sceneLoadHandle
                       .Result
                       .ActivateAsync();
                op.allowSceneActivation = allowSceneBeActive;

                await op;
 
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
        UnloadSceneAsync().Forget();
    }

    public UniTask UnloadSceneAsync()
    {
   
        var handler = Addressables.UnloadSceneAsync(_sceneLoadHandle,autoReleaseHandle:false);
         Addressables.Release(_sceneLoadHandle);
        _currentScene = default;
        _sceneLoadHandle = default;
         return handler.ToUniTask();
    }


}
}