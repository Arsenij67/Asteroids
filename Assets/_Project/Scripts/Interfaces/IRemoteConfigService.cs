using Cysharp.Threading.Tasks;
using System;

namespace Asteroid.Services.RemoteConfig
{
    public interface IRemoteConfigService
    {
        public bool IsInitialized { get; }

        public UniTask Initialize();

        public event Action OnConfigUpdated;
        public T GetValue<T>(string key);

        public UniTask FetchAndActivateAsync();

    }
}
