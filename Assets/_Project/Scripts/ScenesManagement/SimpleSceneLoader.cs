using Asteroid.Generation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SimpleSceneLoader : ISceneLoader
{
    private AsyncOperation asyncLoading;
    private const float LEVEL_LOAD_ADDITIVE_SCENE = 0.9f;
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
       return UniTask.WaitUntil(() => asyncLoading.progress >= LEVEL_LOAD_ADDITIVE_SCENE);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SwitchSceneActivation(bool allowSceneBeActive)
    {
        asyncLoading.allowSceneActivation = allowSceneBeActive;
    }
}
