
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
        if (_sceneLoadHandle.IsValid())
        {
            await UnloadSceneAsync();
        }

        _lastSceneName = name;
        _sceneLoadHandle = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive, allowSceneActivate);
        _currentScene = await _sceneLoadHandle.ToUniTask();
    }

    public void ReloadScene(string nameId)
    {
        Scene sceneData = SceneManager.GetSceneByName(nameId);
        if (!sceneData.isLoaded)
        {
                Debug.Log("Scene loaded");
            LoadScene(sceneData.name);
        }
    }

    public void SwitchSceneActivation(bool allowSceneBeActive)
    {
        if (IsSceneReady())
        {
            Debug.Log(9);
            var operation = _sceneLoadHandle
                .Result
                .ActivateAsync();
            operation.allowSceneActivation = allowSceneBeActive;
            return;
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
        return handler.ToUniTask();
    }

}
}