using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LocalBundleSceneLoader : ISceneLoader
{
    private const float MAX_LOADING_LEVEL = 1f;

    private SceneInstance _currentScene;
    private AsyncOperationHandle<SceneInstance> _sceneLoadHandle;
    private string _lastSceneName = string.Empty;
    public float LoadingProgress => _sceneLoadHandle.IsValid() ? _sceneLoadHandle.PercentComplete : 0f;
    public string LastLoadedScene => _sceneLoadHandle.IsValid() && _sceneLoadHandle.PercentComplete >= MAX_LOADING_LEVEL ? _lastSceneName : string.Empty;

    public void LoadScene(string name)
    {
        _lastSceneName = name;
        LoadSceneAsync(name).Forget();
    }

    public void LoadSceneAdditive(string name)
    {
        _lastSceneName = name;
        LoadSceneAdditiveAsync(name).Forget();
    }

    public async UniTask LoadSceneAsync(string name)
    {
        _lastSceneName = name;

        if (_currentScene.Scene.IsValid() && !_currentScene.Scene.isLoaded)
        {

            _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Single);
            _currentScene = await _sceneLoadHandle.ToUniTask();
        }
    }

    public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
    {
        _lastSceneName = name;
        _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive,allowSceneActivate);
        _sceneLoadHandle.Completed += handle =>
        {
            _currentScene = handle.Result;
        };

       return _sceneLoadHandle.ToUniTask();
    }

    public void ReloadScene(string nameId)
    {
        _lastSceneName = nameId;
        LoadScene(nameId);
    }

    public void SwitchSceneActivation(bool allowSceneBeActive)
    {
        if (_sceneLoadHandle.IsValid() && _sceneLoadHandle.IsDone)
        {
            _sceneLoadHandle.Result.ActivateAsync().allowSceneActivation = allowSceneBeActive;
        }
    }

    public void UnloadScene()
    {
        UnloadSceneAsync().Forget();
    }

    public UniTask UnloadSceneAsync()
    {
        var handler = Addressables.UnloadSceneAsync(_currentScene, UnityEngine.SceneManagement.UnloadSceneOptions.None);
        return handler.ToUniTask();
    }
}