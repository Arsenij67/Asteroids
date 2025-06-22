using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public interface ISceneUnloader
    {
        public void UnloadScene(string sceneName);

        public UniTask UnloadSceneAsync(string sceneName);
    }

}