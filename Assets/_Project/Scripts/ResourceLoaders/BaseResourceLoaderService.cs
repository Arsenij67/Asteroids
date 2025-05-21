using UnityEngine;

namespace Asteroid.Generation
{
    public class BaseResourceLoaderService : IResourceLoaderService
    {
        public GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return null;
            }
            return Object.Instantiate(prefab,parent);
        }

        public GameObject Instantiate(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            if (rotation == null)
            {
                return Instantiate(prefab, position,Quaternion.identity);
            }
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return null;
            }
            return Object.Instantiate(prefab, position, rotation);
        }

        public T LoadResource<T>(string path) where T : Object
        {
            T result = Resources.Load<T>(path);

            if (result == null)
            {
                Debug.LogError($"Resource not found at path: {path}");
               
            }
            return result;
        }
    }
}
