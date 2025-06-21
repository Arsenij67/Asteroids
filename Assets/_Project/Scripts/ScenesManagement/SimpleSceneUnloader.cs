using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroid.Generation
{
    public class SimpleSceneUnloader : ISceneUnloader
    {
        public bool UnloadScene(string sceneName)
        {
            return SceneManager.UnloadScene(sceneName);
        }

        public UniTask UnloadSceneAsync(string sceneName)
        {
           int buildIndex =  SceneManager.GetSceneByName(sceneName).buildIndex;
           AsyncOperation operationUnload =  SceneManager.UnloadSceneAsync(buildIndex);
           return operationUnload.ToUniTask();
        }
    }
}
