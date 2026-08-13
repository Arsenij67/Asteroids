using Asteroid.Database.Connection;
using Asteroid.Generation;
using Asteroid.Services.UnityCloud;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

namespace Asteroid.Database
{
    public class SaveDataStrategyManager : IDisposable
    {
        private bool ChoiceIsMade => _saveModeUI?.ChoiceIsMade ?? false;

        private SaveChoice _saveModeChoice;
        private bool _initialized=false;
        private SaveStrategy[] _saveStrategies;
        private SaveStrategy _currentSaveStrategy;
        private SaveModeUI _saveModeUIPrefab;
        private RectTransform _parentForUI;
        private SaveModeUI? _saveModeUI;
        private LocalSaveStrategy _localSavePresenter;
        private CloudDataPresenter _cloudSavePresenter;
        private IResourceLoader _resourceLoaderService;
        private IInstanceCreator _instanceCreator;
        private WIFIConnector _WIFIConnector;

        public async UniTask Initialize(WIFIConnector wifiConnector, IInstanceCreator instanceCreator, IResourceLoader resourceLoaderService, SaveModeUI saveModeUIPrefab, RectTransform parentForUI, params SaveStrategy[] saveStrategies)
        {
            _saveStrategies = saveStrategies;
            _saveModeUIPrefab = saveModeUIPrefab;
            _resourceLoaderService = resourceLoaderService;
            _parentForUI = parentForUI;
            _WIFIConnector = wifiConnector;
            _instanceCreator = instanceCreator;
            _cloudSavePresenter = _saveStrategies.FirstOrDefault((strategy) => strategy is CloudDataPresenter) as CloudDataPresenter;
            _localSavePresenter = _saveStrategies.FirstOrDefault((strategy) => strategy is LocalSaveStrategy) as LocalSaveStrategy;

            if (!_initialized)
            {
                _initialized = true;
            }

            await DefineStrategy(_saveModeChoice);

            await DefineTypeConnectionWaiting();

            _WIFIConnector.OnInternetConnected += TryOpenWindowSaveMode;
            _WIFIConnector.OnInternetConnected += DefineStrategy;
            _WIFIConnector.OnInternetConnected += DefineTypeConnectionWaiting;
            _WIFIConnector.OnInternetDisconnected += DefineStrategy;
            _WIFIConnector.OnInternetDisconnected += DefineTypeConnectionWaiting;
        }

        private UniTask TrySynchronizeData(SaveStrategy cloudStrategy,SaveStrategy localStrategy)
        {
            bool localSaveMoreThanCloudSave = cloudStrategy.LastSaveTime < localStrategy.LastSaveTime;

            if (_WIFIConnector.IsConnected && localSaveMoreThanCloudSave)
            {
                DataSave synchronizedLocalData = FillUpDataSave(localStrategy);
                return cloudStrategy.UpdateAllData(synchronizedLocalData);
            }

            else if (!localSaveMoreThanCloudSave)
            {
                DataSave synchronizedCloudData = FillUpDataSave(cloudStrategy);
                return localStrategy.UpdateAllData(synchronizedCloudData);
            }
            return UniTask.CompletedTask;
        }

        public async UniTask UpdateCoins(int coinsToAdd)
        {
            await _currentSaveStrategy.AddCountCoins(coinsToAdd);
            _currentSaveStrategy.UpdateUICountCoins(_currentSaveStrategy.CountCoins);
        }

        public async UniTask UpdateNoAds(bool isCanceled)
        {
            await _currentSaveStrategy.UpdateNoAdsStatus(isCanceled);
            _currentSaveStrategy.UpdateUINoAds(isCanceled);
        }

        public async UniTask UpdateDestroyedEnemies(int enemiesToAdd)
        {
            await _currentSaveStrategy.AddCountDeadEnemies(enemiesToAdd);
            _currentSaveStrategy.UpdateUIDeadEnemies();
        }

        public void Dispose()
        {
            _WIFIConnector.Dispose();
            _WIFIConnector.OnInternetConnected -= TryOpenWindowSaveMode;
            _WIFIConnector.OnInternetConnected -= DefineStrategy;
            _WIFIConnector.OnInternetConnected -= DefineTypeConnectionWaiting;
            _WIFIConnector.OnInternetDisconnected -= DefineStrategy;
            _WIFIConnector.OnInternetDisconnected -= DefineTypeConnectionWaiting;
        }

        private DataSave FillUpDataSave(SaveStrategy strategy)
        {
            DataSave synchronizeData = _instanceCreator.CreateInstance<DataSave>();
            synchronizeData[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = strategy.CountDeadEnemies;
            synchronizeData[KeyData.COINS_COUNT] = strategy.CountCoins;
            synchronizeData[KeyData.ADS_DISABLED] = strategy.NoAdsStatus;
            synchronizeData[KeyData.LAST_SAVE_TIME] = strategy.LastSaveTime;
            return synchronizeData;
        }

        private async UniTask DefineStrategy(SaveChoice newSaveChoice = SaveChoice.NoChoice)
        {
            _saveModeChoice = newSaveChoice;

             await _WIFIConnector.IsConnectionAvailable();
             await TrySynchronizeData(_cloudSavePresenter, _localSavePresenter);

            if (newSaveChoice.Equals(SaveChoice.UseCloud) && _WIFIConnector.IsConnected)
            {
                _currentSaveStrategy = _cloudSavePresenter;

            }

            else if (newSaveChoice.Equals(SaveChoice.UseLocal))
            {
                _currentSaveStrategy = _localSavePresenter;

            }

            else if (_WIFIConnector.IsConnected)
            {
                _currentSaveStrategy = _cloudSavePresenter;
            }

            else
            {
                _currentSaveStrategy = _localSavePresenter;
            }

            _currentSaveStrategy.UpdateAllDataUI();
        }

        private async UniTask DefineTypeConnectionWaiting()
        {
            await _WIFIConnector.IsConnectionAvailable();
     
            if (_WIFIConnector.IsConnected)
            {
                _WIFIConnector.WaitForDisconnection();
            }

            else
            {
                _WIFIConnector.WaitForConnection();
            }
        }

       private async UniTask TryOpenWindowSaveMode()
        {
            if (!ChoiceIsMade)
            {
                _saveModeUI = await _resourceLoaderService.InstantiateAsync<SaveModeUI>(_saveModeUIPrefab, _parentForUI);
                _saveModeUI.Initialize();
                _saveModeUI.OnButtonApplyPressed += DefineStrategy;
                _saveModeUI.OnButtonClosePressed += CloseWindowSaveMode;
            }
        }

        private UniTask DefineStrategy()
        {
            return DefineStrategy(_saveModeChoice);
        }

        private void CloseWindowSaveMode()
        {
            if (ChoiceIsMade)
            {
                _saveModeUI.OnButtonClosePressed -= CloseWindowSaveMode;
                _saveModeUI.OnButtonApplyPressed -= DefineStrategy;
                _saveModeUI?.CloseWindow();

                if (_resourceLoaderService.IsResourceLoaded(_saveModeUIPrefab.name))
                {
                    _resourceLoaderService.UnloadResource(_saveModeUIPrefab.name);
                }
            }
        }
    }
}