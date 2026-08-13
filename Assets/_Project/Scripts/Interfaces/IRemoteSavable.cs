using Asteroid.Database;
using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Asteroid.Database
{

    public interface IRemoteSavable
    {
        public bool IsInitialized { get; }
        public UniTask Initialize(DataSave dataSave, WIFIConnector wIFIConnector);
        UniTask SaveKey(string key, object value);
        UniTask<T> GetKey<T>(string key);
        UniTask<DateTime> GetTimeLastModified();
    }
}
