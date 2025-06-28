using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : ISceneLoader
{
    private const float LEVEL_LOAD_ADDITIVE_SCENE = 0.9f;

    private AsyncOperation asyncLoading;
    private string _sceneName;

    public float LoadingProgress => asyncLoading== null ? 0: asyncLoading.progress;

    public string LastLoadedScene => _sceneName;

    private bool SceneForUnloadingIsValid
    {
        get
        {
            Scene scene = SceneManager.GetSceneByName(_sceneName);
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

    public UniTask LoadSceneAsync(string name)
    {
       asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
       return asyncLoading.ToUniTask().ContinueWith(() => { return asyncLoading.isDone; });
        
    }

    public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
    {
       Debug.Log(0);
       asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
       asyncLoading.allowSceneActivation = allowSceneActivate;
       return UniTask.WaitUntil(() => asyncLoading.progress >= LEVEL_LOAD_ADDITIVE_SCENE).ContinueWith(() => Debug.Log($"Scene {name} succesfuly loaded additive! Progress :{asyncLoading.progress}"));
    }

    public void ReloadScene(string nameId)
    {
        Scene sceneData = SceneManager.GetSceneByName(nameId);
        if (!sceneData.isLoaded)
        {
            SceneManager.LoadScene(sceneData.name);
        }
    }

    public void SwitchSceneActivation(bool allowSceneBeActive)
    {
        asyncLoading.allowSceneActivation = allowSceneBeActive;
    }
    public void UnloadScene()
    {
        if (SceneForUnloadingIsValid)
        {
            SceneManager.UnloadSceneAsync(_sceneName);
        }
        else
        {
            Debug.LogWarning($"Cannot unload {_sceneName}: it's the only loaded scene.");
        }
    }

    public UniTask UnloadSceneAsync()
    {
        if (!SceneForUnloadingIsValid)
        {
            Debug.LogWarning($"Cannot unload {_sceneName}: it's invalid or the only loaded scene.");
            return UniTask.CompletedTask;
        }
        AsyncOperation operation = SceneManager.UnloadSceneAsync(_sceneName);
        return operation.ToUniTask();
    }
}

