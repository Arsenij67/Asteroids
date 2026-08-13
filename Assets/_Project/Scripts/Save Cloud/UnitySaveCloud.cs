using Asteroid.Database;
using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Asteroid.Services.UnityCloud
{
    public class UnitySaveCloud : IRemoteSavable
    {
        public bool IsInitialized => _WIFIConnector.IsConnected && _isInitialized;

        private bool _isInitialized = false;
        private bool _isInitializing = false;

        private Dictionary<string, Item> _data;
        private DataSave _dataSave;
        private WIFIConnector _WIFIConnector;

        public async UniTask Initialize(DataSave dataSave, WIFIConnector wIFIConnector)
        {
            _WIFIConnector = wIFIConnector;

            await _WIFIConnector.IsConnectionAvailable();

            if (!_WIFIConnector.IsConnected || _isInitialized || _isInitializing) return;

            _isInitializing = true;
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
            if (!IsInitialized)
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
            if (!IsInitialized)
            {
                UnityEngine.Debug.LogWarning("Попытка загрузить данные до инициализации Cloud Save");
                return default(T);
            }

            HashSet<string> keysHash = new HashSet<string>()
            {
                key
            };
            try
            {

                Dictionary<string, Item> stringDictionary = await CloudSaveService.Instance.Data.Player.LoadAsync(keysHash).AsUniTask();

                if (stringDictionary.TryGetValue(key, out var keyName))
                {
                    return keyName.Value.GetAs<T>();
                }
                else
                {
                    return default(T);
                }
            }

            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async UniTask<DateTime> GetTimeLastModified()
        {
            if (!IsInitialized)
            {
                UnityEngine.Debug.Log("Попытка получить время обновления до инициализации Cloud Save");
                return DateTime.MinValue;
            }

            await DownloadAllData();

            return _data.Values
                .Where(item => item?.Modified.HasValue == true)
                .Select(item => item.Modified.Value)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max().ToLocalTime();
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

            if (!AuthenticationService.Instance.IsSignedIn)
            {

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                UnityEngine.Debug.Log("Анонимная аутентификация завершена успешно!");
            }
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