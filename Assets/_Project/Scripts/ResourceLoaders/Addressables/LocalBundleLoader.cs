using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalBundleLoader :IResourceLoader
{
    public T Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null");
            return null;
        }
      
        var handle = Addressables.InstantiateAsync(prefab.name, position, rotation).WaitForCompletion().GetComponent<T>();
        return handle;
    }

    public T  Instantiate<T>(T prefab, Transform parent) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null");
            return null;
        }

        var handle = Addressables.InstantiateAsync(prefab.name, parent).WaitForCompletion();
        return handle.GetComponent<T>();
    }

    public async UniTask<T> InstantiateAsync<T>(T prefab, Transform parent) where T : Component
    {
        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(prefab.name, parent,trackHandle:true);
        GameObject createdObject = await asyncOperation;
        return createdObject.GetComponent<T>();
    }

    public async UniTask<T> LoadResourceAsync<T>(string path) where T : Object
    {
        if (File.Exists(path))
        {
            Debug.LogError($"resource is null");
            return default(T);
        }

        var resourceHandle = Addressables.LoadAssetAsync<T>(path);
        T createdResource =  await resourceHandle;
        Addressables.Release(resourceHandle);
        return createdResource;
    }

    public T LoadResource<T>(string path) where T : Object
    {
        if (File.Exists(path))
        {
            Debug.LogError($"resource is null");
            return default(T);
        }

        var resource = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        return resource;
    }
}