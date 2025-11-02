using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;

namespace Asteroid.Services.UnityCloud
{
    public class  UnitySaveCloud : IRemoteSavable
    {
        private DataSave _dataSave;
        public UniTask Initialize(DataSave dataSave)
        {
            _dataSave = dataSave;
            return SetUp().ContinueWith(() => SigIn());
        }

        public UniTask SaveKey (string key, object value)
        {
            _dataSave[key] = value;
            var dictionaryToSave = new Dictionary<string, object>()
            {
                { key, value }
            };
            return CloudSaveService.Instance.Data.Player.SaveAsync(dictionaryToSave).AsUniTask();
        }

        public async UniTask<T> GetKey<T>(string key)
        {
            HashSet<string> keysHash = new HashSet<string>()
            {
                key
            };
            var stringDict = await CloudSaveService.Instance.Data.Player.LoadAsync(keysHash).AsUniTask();

            if (stringDict.TryGetValue(key, out var keyName))
            {
                return keyName.Value.GetAs<T>();
            }

            else
            {
                return default(T);
            }
        }

        private UniTask SigIn()
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                return UniTask.CompletedTask;
            }
            return AuthenticationService.Instance.SignInAnonymouslyAsync().AsUniTask();
        }

        private UniTask SetUp()
        {
            if (UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                return UniTask.CompletedTask;
            }
            return UnityServices.InitializeAsync().AsUniTask();
        }
    }
}
