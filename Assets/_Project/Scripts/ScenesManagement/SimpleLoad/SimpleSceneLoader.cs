using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroid.Generation
{
    public class SimpleSceneLoader : ISceneLoader
    {
        private const float LEVEL_LOAD_ADDITIVE_SCENE = 0.9f;

        private AsyncOperation _asyncLoading;

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadSceneAdditive(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }

        public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true)
        {
            _asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            _asyncLoading.allowSceneActivation = allowSceneActivate;
            return UniTask.WaitUntil(() => _asyncLoading.progress >= LEVEL_LOAD_ADDITIVE_SCENE);
        }

        public void ReloadCurrentScene()
        {
            Scene sceneData = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneData.name);
        }

        public UniTask SwitchSceneActivation(string name, bool allowSceneBeActive)
        {
            _asyncLoading.allowSceneActivation = allowSceneBeActive;
            return _asyncLoading.ToUniTask();
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
            _asyncLoading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            _asyncLoading.allowSceneActivation = activateOnLoad;
            return _asyncLoading.ToUniTask();

        }

        public UniTask<object> UnloadSceneAsync(string name)
        {
            if (!SceneForUnloadingIsValid(name))
            {
                Debug.LogWarning($"Cannot unload {name}: it's invalid or the only loaded scene.");
                return UniTask.FromResult<object>(name);
            }
            AsyncOperation operation = SceneManager.UnloadSceneAsync(name);
            return operation.ToUniTask().ContinueWith(() => (object)name);
        }

        public UniTask ReloadSceneAsync(string name)
        {
            return LoadSceneAsync(name, true);
        }

        private bool SceneForUnloadingIsValid(string name)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            return scene.IsValid() && scene.isLoaded && SceneManager.sceneCount > 1;
        }
    }
}