using Cysharp.Threading.Tasks;
using System;
using Asteroid.Database.Connection;

namespace Asteroid.Services.RemoteConfig
{
    public interface IRemoteConfigService
    {
        public event Action OnConfigUpdated;

        public bool IsInitialized { get; }

        public T GetValue<T>(string key);
        public UniTask FetchAndActivateAsync();
        public UniTask Initialize(bool IsServiceAvailable, WIFIConnector wifiConnector);
        public UniTask SetUserAttributes(string[] keys, string[] values);
    }
}
