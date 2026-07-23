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
        public UniTask<GameObject> InstantiateAsync(GameObject prefab, Transform parent)
        {
            AsyncInstantiateOperation<GameObject> asyncOperation = Object.InstantiateAsync(prefab.GameObject(), parent);
            return asyncOperation.ToUniTask().ContinueWith(() => asyncOperation.Result.First());
        }

        public async UniTask<T> LoadResourceAsync<T>(string path) where T : Object
        {
            var handler = Resources.LoadAsync<T>(path).ToUniTask();
            T result = await handler as T;

            if (result == null)
            {
                Debug.LogError($"Resource not found at path: {path}");

            }
            return result;
        }
    }
}
