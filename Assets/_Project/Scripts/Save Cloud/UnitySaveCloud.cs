using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;

namespace Asteroid.Database
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
            return SaveDataAsync(_dataSave);
        }

        public async UniTask<object> GetKey(string key)
        {
            var loadedDictionary = await GetDataAsync();
            return loadedDictionary[key];
        }

        private UniTask SaveDataAsync(DataSave dataSave)
        {
            var dictionaryToSave = new Dictionary<string, object>()
            {
                { CloudKeyData.DEAD_ENEMIES_COUNT, dataSave.CountEnemiesDestroyed.ToString() },
                { CloudKeyData.COINS_COUNT, dataSave.CountCoins.ToString() }

            };
            return CloudSaveService.Instance.Data.Player.SaveAsync(dictionaryToSave).AsUniTask();
        }

        private async UniTask<Dictionary<string, object>> GetDataAsync()
        {
            HashSet<string> keysHash = new HashSet<string>() 
            {
              CloudKeyData.COINS_COUNT,
              CloudKeyData.DEAD_ENEMIES_COUNT
            };

           var stringDict = await CloudSaveService.Instance.Data.Player.LoadAsync(keysHash).AsUniTask();
           Dictionary<string, object> objectDict = stringDict.ToDictionary( x => x.Key,x => (object)x.Value);
           return objectDict;

        }


        private UniTask SigIn()
        {
            return AuthenticationService.Instance.SignInAnonymouslyAsync().AsUniTask().ContinueWith(() => SaveDataAsync(_dataSave));
        }

        private UniTask SetUp()
        {
            return UnityServices.InitializeAsync().AsUniTask();
        }
        
    }
}
