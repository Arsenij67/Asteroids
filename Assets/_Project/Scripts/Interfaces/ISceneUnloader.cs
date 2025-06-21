using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public interface ISceneUnloader
    {
        public bool UnloadScene(string sceneName);

        public UniTask UnloadSceneAsync(string sceneName);
    }

}