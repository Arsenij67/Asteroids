using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public interface IResourceLoaderService
    {
        public T LoadResource<T>(string path) where T : Object;
        public GameObject Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Object;
        public GameObject Instantiate<T>(T prefab,Transform transform) where T : Object;
        public UniTask<GameObject> InstantiateAsync<T>(T prefab,Transform transform) where T : Object;

    }
}
