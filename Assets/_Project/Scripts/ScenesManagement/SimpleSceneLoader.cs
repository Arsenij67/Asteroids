using Asteroid.Generation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SimpleSceneLoader : ISceneLoader
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);   
    }

    public UniTask LoadSceneAsync(string name)
    {
       AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

       return asyncLoading.ToUniTask().ContinueWith(() => { return asyncLoading.isDone; });
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
