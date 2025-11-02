using Asteroid.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Database
{

    public interface IRemoteSavable
    {
        UniTask Initialize(DataSave dataSave);
        UniTask SaveKey(string key, object value);
        UniTask<T> GetKey<T>(string key);
    }
}
