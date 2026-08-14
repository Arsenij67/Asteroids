using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public class RemoteBundleLoader : IResourceLoader
    {
        public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public T Instantiate<T>(T prefab, Transform transform) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public UniTask<T> InstantiateAsync<T>(T prefab, Transform transform) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public bool IsResourceLoaded(string path)
        {
            throw new System.NotImplementedException();
        }

        public T LoadResource<T>(string path) where T : Object
        {
            throw new System.NotImplementedException();
        }

        public UniTask<T> LoadResourceAsync<T>(string path) where T : Object
        {
            throw new System.NotImplementedException();
        }

        public void UnloadAllResources()
        {
            throw new System.NotImplementedException();
        }

        public void UnloadResource(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}