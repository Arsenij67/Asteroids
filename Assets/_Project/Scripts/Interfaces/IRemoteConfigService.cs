using Cysharp.Threading.Tasks;
using System;

namespace Asteroid.Services.RemoteConfig
{
    public interface IRemoteConfigService
    {
        public event Action OnConfigUpdated;

        public bool IsInitialized { get; }

        public T GetValue<T>(string key);
        public UniTask FetchAndActivateAsync();
        public UniTask Initialize();
        public UniTask SetUserAttributes(string[] keys, string[] values);
    }
}
