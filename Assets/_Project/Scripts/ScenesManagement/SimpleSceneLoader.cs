using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : ISceneLoader
{
    private const float LEVEL_LOAD_ADDITIVE_SCENE = 0.9f;

    private AsyncOperation asyncLoading;

    public float LoadingProgress => asyncLoading== null ? 0: asyncLoading.progress;

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

    public UniTask LoadSceneAsyncAdditive(string name, bool allowSceneActivate = true)
    {
       asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
       asyncLoading.allowSceneActivation = allowSceneActivate;
        return UniTask.WaitUntil(() => asyncLoading.progress >= LEVEL_LOAD_ADDITIVE_SCENE).ContinueWith(() => Debug.Log($"Scene {name} succesfuly loaded additive!"));
    }

    public void ReloadScene()
    {
        Scene sceneData = SceneManager.GetActiveScene();

        if (!sceneData.isLoaded)
        {
            SceneManager.LoadScene(sceneData.name);
        }
    }

    public void SwitchSceneActivation(bool allowSceneBeActive)
    {
        asyncLoading.allowSceneActivation = allowSceneBeActive;
    }
}
