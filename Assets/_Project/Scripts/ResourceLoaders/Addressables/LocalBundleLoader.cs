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

    public async UniTask<GameObject> InstantiateAsync<T>(T prefab, Transform parent) where T : Object
    {
        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(prefab.name, parent);
        GameObject createdObject = await asyncOperation;
        Addressables.ReleaseInstance(asyncOperation);
        asyncOperation = default;
        return createdObject;
    }

    public T LoadResource<T>(string path) where T : Object
    {
        if (File.Exists(path))
        {
            Debug.LogError($"Prefab is null");
            return null;
        }

        var resourceReference = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        return resourceReference;
    }
}