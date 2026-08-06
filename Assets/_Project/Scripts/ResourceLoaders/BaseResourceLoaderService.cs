using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroid.Generation
{
    public class BaseResourceLoaderService : IResourceLoader
    {
        public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component
        {
            return InstantiateGameObject(prefab.GameObject(), position,rotation).GetComponent<T>();
        }
        public T Instantiate<T>(T prefab, Transform transform) where T : Component
        {
            return InstantiateGameObject(prefab.GameObject(), transform).GetComponent<T>();    
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

        public UniTask<T> InstantiateAsync <T>(T prefab, Transform parent) where T : Component  
        {
            AsyncInstantiateOperation<GameObject> asyncOperation = Object.InstantiateAsync(prefab.GameObject(), parent);
            return asyncOperation.ToUniTask().ContinueWith(() => asyncOperation.Result.First().GetComponent<T>());
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
                return Object.Instantiate(prefab, position, Quaternion.identity);
            }
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return null;
            }
            return Object.Instantiate(prefab.GameObject(), position, rotation);
        }
    }
}
