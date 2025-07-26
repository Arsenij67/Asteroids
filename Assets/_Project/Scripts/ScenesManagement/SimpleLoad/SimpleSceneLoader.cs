using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : ISceneLoader
{
    private const float LEVEL_LOAD_ADDITIVE_SCENE = 0.9f;

    private AsyncOperation asyncLoading;
    private string _lastSceneName;

    public float LoadingProgress => asyncLoading == null ? 0: asyncLoading.progress;

    public string LastLoadedScene => _lastSceneName;

    private bool SceneForUnloadingIsValid
    {
        get
        {
            Scene scene = SceneManager.GetSceneByName(_lastSceneName);
            return scene.IsValid() && scene.isLoaded && SceneManager.sceneCount > 1;
        }
    }

    public void LoadScene(string name)
    {
       SceneManager.LoadScene(name);   
    }

    public void LoadSceneAdditive(string name)
    {
       SceneManager.LoadScene(name,LoadSceneMode.Additive);
    }

    public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
    {
       asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
       asyncLoading.allowSceneActivation = allowSceneActivate;
       return UniTask.WaitUntil(() => asyncLoading.progress >= LEVEL_LOAD_ADDITIVE_SCENE);
    }

    public void ReloadScene()
    {
        Scene sceneData = SceneManager.GetActiveScene();
        _lastSceneName = sceneData.name;    
        SceneManager.LoadScene(sceneData.name);
    }

    public UniTask SwitchSceneActivation(bool allowSceneBeActive)
    {
        asyncLoading.allowSceneActivation = allowSceneBeActive;
        return asyncLoading.ToUniTask();
    }
    public void UnloadScene()
    {
        if (SceneForUnloadingIsValid)
        {
            SceneManager.UnloadSceneAsync(_lastSceneName);
        }
        else
        {
            Debug.LogWarning($"Cannot unload {_lastSceneName}: it's the only loaded scene.");
        }
    }

    public UniTask<object> ReloadSceneAsync(string name)
    {
        Scene sceneData = SceneManager.GetSceneByName(name);
        if (!sceneData.isLoaded)
        {
          AsyncOperation sceneHandler = SceneManager.LoadSceneAsync(sceneData.name);
          return sceneHandler.ToUniTask().ContinueWith(() => { return (object) name; });
        }
        _lastSceneName = sceneData.name;
        return UniTask.FromResult<object>(_lastSceneName);
    }

    public UniTask<object> LoadSceneAsync(string name)
    {
        asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        _lastSceneName = name;
        return asyncLoading.ToUniTask().ContinueWith(() => { return (object) name; });
      
    }

    public UniTask<object> UnloadSceneAsync(object data)
    {
        _lastSceneName = (string)data;
        if (!SceneForUnloadingIsValid)
        {
            Debug.LogWarning($"Cannot unload {_lastSceneName}: it's invalid or the only loaded scene.");
            return UniTask.FromResult<object>(_lastSceneName);
        }
        AsyncOperation operation = SceneManager.UnloadSceneAsync(_lastSceneName);
        return operation.ToUniTask().ContinueWith(()=>(object)_lastSceneName);
    }
}

