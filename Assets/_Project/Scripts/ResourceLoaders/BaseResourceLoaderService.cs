using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroid.Generation
{
    public class BaseResourceLoaderService : IResourceLoaderService
    {
        public GameObject Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Object
        {
            return InstantiateGameObject(prefab.GameObject(), position,rotation);
        }
        public GameObject Instantiate<T>(T prefab, Transform transform) where T : Object
        {
            return InstantiateGameObject(prefab.GameObject(), transform);    
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

        public T CreateInstance<T>() where T : new()
        {
            return new T();
        }
        private GameObject InstantiateGameObject(GameObject prefab, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return null;
            }
            return Object.Instantiate(prefab.GameObject(), parent);
        }

        private GameObject InstantiateGameObject(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            if (rotation == null)
            {
                return Instantiate(prefab, position, Quaternion.identity);
            }
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return null;
            }
            return Object.Instantiate(prefab.GameObject(), position, rotation);
        }
        public UniTask<GameObject> InstantiateAsync<T>(T prefab, Transform parent) where T : Object
        {
            AsyncInstantiateOperation<GameObject> asyncOperation = Object.InstantiateAsync(prefab.GameObject(), parent);
            return asyncOperation.ToUniTask().ContinueWith(() => asyncOperation.Result.First());
        }
    }
}
