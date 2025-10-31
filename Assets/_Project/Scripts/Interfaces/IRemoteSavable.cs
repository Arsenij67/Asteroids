using Asteroid.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IRemoteSavable
{
    public UniTask Initialize(DataSave dataSave);
    public UniTask SaveKey(string key, object value);
    public UniTask<object> GetKey(string key);
}
