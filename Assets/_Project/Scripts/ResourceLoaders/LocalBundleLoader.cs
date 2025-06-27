using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalBundleLoader : IResourceLoaderService
{

    public GameObject Instantiate<T>(T prefab, Vector2 position, Quaternion rotation) where T : Object
    {
        return null;
    }

    public GameObject Instantiate<T>(T prefab, Transform transform) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public UniTask<GameObject> InstantiateAsync<T>(T prefab, Transform parent) where T : Object
    {
        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(prefab, parent);
        return asyncOperation.Task.AsUniTask(); 
    }

    public T LoadResource<T>(string path) where T : Object
    {
        throw new System.NotImplementedException();
    }
}
