using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroid.Generation
{
    public class BaseResourceLoaderService : IResourceLoader
    {
        private readonly Dictionary<string, ResourceRequest> _resourceRequests = new();

        public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("[BaseResourceLoaderService] Prefab is null");
                return null;
            }

            GameObject instance = Object.Instantiate(prefab.gameObject, position, rotation);
            return instance.GetComponent<T>();
        }

        public T Instantiate<T>(T prefab, Transform parent) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("[BaseResourceLoaderService] Prefab is null");
                return null;
            }

            GameObject instance = Object.Instantiate(prefab.gameObject, parent);
            return instance.GetComponent<T>();
        }

        public T LoadResource<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[BaseResourceLoaderService] Path is null or empty");
                return null;
            }

            T result = Resources.Load<T>(path);

            if (result == null)
            {
                Debug.LogError($"[BaseResourceLoaderService] Resource not found at path: {path}");
            }

            return result;
        }

        public async UniTask<T> InstantiateAsync<T>(T prefab, Transform parent) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("[BaseResourceLoaderService] Prefab is null");
                return null;
            }

            try
            {
                var asyncOperation = Object.InstantiateAsync(prefab.gameObject, parent);
                await asyncOperation.ToUniTask();
                var results = asyncOperation.Result;
                if (results == null || results.Length == 0)
                {
                    Debug.LogError($"[BaseResourceLoaderService] InstantiateAsync failed for {prefab.name}");
                    return null;
                }
                GameObject instance = results[0];
                return instance.GetComponent<T>();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[BaseResourceLoaderService] InstantiateAsync error: {ex.Message}");
                return null;
            }
        }

        public async UniTask<T> LoadResourceAsync<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[BaseResourceLoaderService] Path is null or empty");
                return null;
            }

            try
            {
                ResourceRequest request = Resources.LoadAsync<T>(path);

                if (!_resourceRequests.ContainsKey(path))
                {
                    _resourceRequests[path] = request;
                }

                await request.ToUniTask();

                T result = request.asset as T;

                if (result == null)
                {
                    Debug.LogError($"[BaseResourceLoaderService] Resource not found at path: {path}");
                    _resourceRequests.Remove(path);
                    return null;
                }

                return result;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[BaseResourceLoaderService] LoadResourceAsync error for {path}: {ex.Message}");
                return null;
            }
        }

        public void UnloadResource (string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[BaseResourceLoaderService] Cannot unload: path is null or empty");
                return;
            }

            try
            {
                if (_resourceRequests.TryGetValue(path, out var request))
                {
                    if (request.asset != null)
                    {
                        Resources.UnloadAsset(request.asset);
                        Debug.Log($"[BaseResourceLoaderService] Unloaded: {path}");
                    }
                    _resourceRequests.Remove(path);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[BaseResourceLoaderService] Unload error for {path}: {ex.Message}");
            }
        }

        public void UnloadAllResources()
        {
            try
            {
                foreach (var keyValuePairHandle in _resourceRequests)
                {
                    if (keyValuePairHandle.Value?.asset != null)
                    {
                        Resources.UnloadAsset(keyValuePairHandle.Value.asset);
                    }
                }
                Resources.UnloadUnusedAssets();
                _resourceRequests.Clear();

                Debug.Log("[BaseResourceLoaderService] Unloaded all resources");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[BaseResourceLoaderService] UnloadAll error: {ex.Message}");
            }
        }

        public bool IsResourceLoaded(string path)
        {
            return _resourceRequests.ContainsKey(path) && _resourceRequests[path]?.asset != null;
        }
    }
}