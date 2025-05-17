using UnityEngine;

namespace Asteroid.Generation
{
    public interface IResourceLoaderService
    {
        public T LoadResource<T>(string path) where T : UnityEngine.Object;
        public GameObject Instantiate(GameObject prefab,Transform parent = null);
        public GameObject Instantiate(GameObject prefab,Vector2 position, Quaternion rotation);
        public T CreateInstance<T>() where T : new()
        {
            return new T();
        }
    }
}
