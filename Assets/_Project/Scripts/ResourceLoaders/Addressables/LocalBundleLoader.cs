using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalBundleLoader : IResourceLoaderService
{
    public GameObject Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Object
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null");
            return null;
        }
      
        var handle = Addressables.InstantiateAsync(prefab.name, position, rotation).WaitForCompletion();
        return handle;
    }

    public GameObject Instantiate<T>(T prefab, Transform parent) where T : Object
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null");
            return null;
        }

        var handle = Addressables.InstantiateAsync(prefab.name, parent).WaitForCompletion();
        return handle;
    }

    public async UniTask<GameObject> InstantiateAsync(GameObject prefab, Transform parent)
    {
        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(prefab.name, parent,trackHandle:true);
        GameObject createdObject = await asyncOperation;
        return createdObject;
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