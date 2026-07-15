using Asteroid.Database;
using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

namespace Asteroid.Services.UnityCloud
{
    public class UnitySaveCloud : Connector, IRemoteSavable
    {
        private Dictionary<string, Item> _data;
        private DataSave _dataSave;

        private bool IsInitialized => IsConnected && _isInitialized;

        private bool _isInitialized = false;

        public async UniTask Initialize(DataSave dataSave)
        {
            await IsConnectionAvailable();

            if (!IsInitialized) return;

            _dataSave = dataSave;

            try
            {
                await SetUp();
                await SignIn();
                _isInitialized = true;
                UnityEngine.Debug.Log("Unity Cloud Save инициализирован успешно!");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log($"Ошибка инициализации Unity Cloud Save: {ex.Message}");
                throw;
            }
            finally
            {
                _isInitialized = false;
            }
                await DownloadAllData();
        }

        private async UniTask DownloadAllData()
        {
            _data =  await CloudSaveService.Instance.Data.Player.LoadAllAsync();

            foreach (string key in _data.Keys)
            {
                _dataSave[key] = _data[key].Value.GetAsString();
            }
        }

        public UniTask SaveKey(string key, object value)
        {
            if (!_isInitialized)
            {
                UnityEngine.Debug.LogWarning("Попытка сохранить данные до инициализации Cloud Save");
                return UniTask.CompletedTask;
            }

            var dictionaryToSave = new Dictionary<string, object>()
            {
                { key, value }
            };
            return CloudSaveService.Instance.Data.Player.SaveAsync(dictionaryToSave).AsUniTask();
        }

        public async UniTask<T> GetKey<T>(string key)
        {
            if (!_isInitialized)
            {
                UnityEngine.Debug.LogWarning("Попытка загрузить данные до инициализации Cloud Save");
                return default(T);
            }

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

        public DateTime GetKeyLastModified(string key)
        {
            if (!IsInitialized)
            {
                UnityEngine.Debug.Log("Попытка получить время обновления до инициализации Cloud Save");
                return DateTime.MinValue;
            }
            return _data[key].Modified.Value.ToLocalTime();
        }

        private async UniTask SignIn()
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                UnityEngine.Debug.Log("Пользователь уже аутентифицирован");
                return;
            }

            while (AuthenticationService.Instance.IsAuthorized && !AuthenticationService.Instance.IsSignedIn)
            {
                await UniTask.NextFrame();
            }

            if (AuthenticationService.Instance.IsSignedIn)
                return;

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                UnityEngine.Debug.Log("Анонимная аутентификация завершена успешно!");
        }

        private async UniTask SetUp()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return;
            }

            if (UnityServices.State == ServicesInitializationState.Initializing)
            {
                while (UnityServices.State == ServicesInitializationState.Initializing)
                {
                    await UniTask.Delay(100);
                }

                if (UnityServices.State == ServicesInitializationState.Initialized)
                {
                    return;
                }
            }

            await UnityServices.InitializeAsync();
            UnityEngine.Debug.Log("Инициализация Unity Services завершена успешно!");
        }
    }
}