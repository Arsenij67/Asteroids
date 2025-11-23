using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

namespace Asteroid.Services.UnityCloud
{
    public class UnitySaveCloud : IRemoteSavable
    {
        private DataSave _dataSave;
        private static bool _isInitializing = false;
        private static bool _isInitialized = false;

        public async UniTask Initialize(DataSave dataSave)
        {
            if (_isInitialized || _isInitializing)
                return;

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
                UnityEngine.Debug.LogError($"Ошибка инициализации Unity Cloud Save: {ex.Message}");
                throw;
            }
            finally
            {
                _isInitializing = false;
            }
            await DownloadAllData();
        }

        private async UniTask DownloadAllData()
        {
           Dictionary<string,Item> data =  await CloudSaveService.Instance.Data.Player.LoadAllAsync();

            foreach (string key in data.Keys)
            {
                _dataSave[key] = data[key].Value.GetAsString();
            }
        }

        public UniTask SaveKey(string key, object value)
        {
            if (!_isInitialized)
            {
                UnityEngine.Debug.LogWarning("Попытка сохранить данные до инициализации Cloud Save");
                return UniTask.CompletedTask;
            }

            _dataSave[key] = value;
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

            // Если уже выполняется инициализация, ждем ее завершения
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