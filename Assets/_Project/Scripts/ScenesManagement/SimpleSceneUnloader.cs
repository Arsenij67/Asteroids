using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroid.Generation
{
    public class SimpleSceneUnloader : ISceneUnloader
    {
        private string _sceneName;

        private bool SceneForUnloadingIsValid
        {
            get
            {
                Scene scene = SceneManager.GetSceneByName(_sceneName);
                return scene.IsValid() && scene.isLoaded && SceneManager.sceneCount > 1;
            }
        }
        public void UnloadScene(string sceneName)
        {
            _sceneName = sceneName;
            if (SceneForUnloadingIsValid)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
            else
            {
                Debug.LogWarning($"Cannot unload {sceneName}: it's the only loaded scene.");
            }
        }

        public UniTask UnloadSceneAsync(string sceneName)
        {
            _sceneName = sceneName;
            if (!SceneForUnloadingIsValid)
            {
                Debug.LogWarning($"Cannot unload {sceneName}: it's invalid or the only loaded scene.");
                return UniTask.CompletedTask;
            }
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            return operation.ToUniTask();
        }
    }
}
