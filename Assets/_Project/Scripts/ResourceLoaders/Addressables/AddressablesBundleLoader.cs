using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableBundleLoader : IResourceLoader, IDisposable
{
    private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

    public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"[AddressablesBundleLoader] Prefab is null");
            return null;
        }

        try
        {
            var handle = Addressables.InstantiateAsync(prefab.name, position, rotation,trackHandle:false);
            var result = handle.WaitForCompletion();
            _handles[prefab.name] = handle;
            return result.GetComponent<T>();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to instantiate {prefab.name}: {ex.Message}");
            return null;
        }
    }

    public T Instantiate<T>(T prefab, Transform parent) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"[AddressablesBundleLoader] Prefab is null");
            return null;
        }

        try
        {
            var handle = Addressables.InstantiateAsync(prefab.name, parent,false,false);
            var result = handle.WaitForCompletion();
            _handles[prefab.name] = handle;
            return result.GetComponent<T>();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to instantiate {prefab.name}: {ex.Message}");
            return null;
        }
    }

    public T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"[AddressablesBundleLoader] Path is null or empty");
            return null;
        }

        try
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            var result = handle.WaitForCompletion();

            if (result == null)
            {
                Debug.LogError($"[AddressablesBundleLoader] Failed to load resource: {path}");
                return null;
            }
            _handles[path] = handle;
            return result;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to load {path}: {ex.Message}");
            return null;
        }
    }

    public async UniTask<T> InstantiateAsync<T>(T prefab, Transform parent) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"[AddressablesBundleLoader] Prefab is null");
            return null;
        }

        try
        {
            var handle = Addressables.InstantiateAsync(prefab.name, parent, false,false);

            var result = await handle;

            if (result == null)
            {
                Debug.LogError($"[AddressablesBundleLoader] Failed to instantiate {prefab.name}");
                return null;
            }

            _handles.Add(prefab.name,handle);

            return result.GetComponent<T>();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to instantiate async {prefab.name}: {ex.Message}");
            return null;
        }
    }

    public async UniTask<T> LoadResourceAsync<T>(string path) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"[AddressablesBundleLoader] Path is null or empty");
            return null;
        }

        try
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            var result = await handle;

            if (result == null)
            {
                Debug.LogError($"[AddressablesBundleLoader] Failed to load resource async: {path}");
                return null;
            }
            _handles[path] = handle;
            return result;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to load async {path}: {ex.Message}");
            return null;
        }
    }

    public void UnloadResource(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"[AddressablesBundleLoader] Cannot unload: path is null or empty");
            return;
        }

        try
        {
            if (_handles.TryGetValue(path, out var handle))
            {
                Addressables.Release(handle);
                _handles.Remove(path);
                Debug.Log($"[AddressablesBundleLoader] Unloaded: {path}");
            }
            else
            {
                Debug.LogWarning($"[AddressablesBundleLoader] Handle not found for: {path}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AddressablesBundleLoader] Failed to unload {path}: {ex.Message}");
        }
    }

    public void UnloadAllResources()
    {
        foreach (var keyValuePairHandle in _handles)
        {
            try
            {
                Addressables.Release(keyValuePairHandle.Value);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[AddressablesBundleLoader] Failed to unload {keyValuePairHandle.Key}: {ex.Message}");
            }
        }

        _handles.Clear();
        Debug.Log($"[AddressablesBundleLoader] Unloaded all resources");
    }

    public bool IsResourceLoaded(string path)
    {
        return _handles.ContainsKey(path) && _handles[path].IsValid();
    }

    public void Dispose()
    {
        if (Caching.ClearCache())
        {
            Debug.LogWarning($"[AddressablesBundleLoader] кэш очищен");
        }

        else
        {
            Debug.LogWarning($"[AddressablesBundleLoader] ошибка очистки кэша");
        }
    }
}