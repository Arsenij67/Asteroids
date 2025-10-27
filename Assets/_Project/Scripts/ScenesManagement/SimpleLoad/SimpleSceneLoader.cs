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

    public void ReloadCurrentScene()
    {
        Scene sceneData = SceneManager.GetActiveScene();   
        SceneManager.LoadScene(sceneData.name);
    }

    public UniTask SwitchSceneActivation(string name, bool allowSceneBeActive)
    {
        asyncLoading.allowSceneActivation = allowSceneBeActive;
        return asyncLoading.ToUniTask();
    }
    public void UnloadScene(string name)
    {
        if (SceneForUnloadingIsValid(name))
        {
            SceneManager.UnloadSceneAsync(name);
        }
        else
        {
            Debug.LogWarning($"Cannot unload {name}: it's the only loaded scene.");
        }
    }

    public UniTask ReloadSceneAsync(string name, bool activateOnLoad = true)
    {
        Scene sceneData = SceneManager.GetSceneByName(name);
        if (!sceneData.isLoaded)
        {
          AsyncOperation sceneHandler = SceneManager.LoadSceneAsync(sceneData.name);
          sceneHandler.allowSceneActivation = activateOnLoad;
            return sceneHandler.ToUniTask();
        }
        name = sceneData.name;
        return UniTask.FromResult<object>(name);
    }

    public UniTask LoadSceneAsync(string name, bool activateOnLoad)
    {
        asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        asyncLoading.allowSceneActivation = activateOnLoad;
        return asyncLoading.ToUniTask();
      
    }

    public UniTask<object> UnloadSceneAsync(string name)
    {
        if (!SceneForUnloadingIsValid(name))
        {
            Debug.LogWarning($"Cannot unload {name}: it's invalid or the only loaded scene.");
            return UniTask.FromResult<object>(name);
        }
        AsyncOperation operation = SceneManager.UnloadSceneAsync(name);
        return operation.ToUniTask().ContinueWith(()=>(object)name);
    }

    private bool SceneForUnloadingIsValid(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        return scene.IsValid() && scene.isLoaded && SceneManager.sceneCount > 1;
    }

    public UniTask ReloadStartSceneAsync(string name)
    {
        return LoadSceneAsync(name, true);
    }
}

