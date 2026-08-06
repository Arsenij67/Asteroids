using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public interface IResourceLoader
    {
        public T LoadResource<T>(string path) where T : Object;
        public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component;
        public T Instantiate<T>(T prefab,Transform transform) where T : Component;
        public UniTask<T> InstantiateAsync<T>(T prefab, Transform transform) where T : Component;
        public UniTask<T> LoadResourceAsync<T>(string path) where T : Object;

    }
}
