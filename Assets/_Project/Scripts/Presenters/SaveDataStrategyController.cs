
using Asteroid.Database.Connection;
using Asteroid.Generation;
using Asteroid.Services.UnityCloud;
using Asteroid.SpaceShip;
using Asteroid.UI;
using Asteroid.Weapon;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

namespace Asteroid.Database
{
    public class SaveDataStrategyController : Connector, IDisposable
    {
        public bool NoAdsStatus => _currentSaveStrategy.NoAdsStatus;
        public int CountCoins => _currentSaveStrategy.CountCoins;

        private bool ChoiceIsMade => _saveModeUI?.ChoiceIsMade ?? false;

        private SaveChoice _saveModeChoice;
        private bool _initialized=false;
        private SaveStrategy[] _saveStrategies;
        private SaveStrategy _currentSaveStrategy;
        private GameObject _saveModeUIPrefab;
        private RectTransform _parentForUI;
        private SaveModeUI? _saveModeUI;
        private LocalSaveStrategyPresenter _localSavePresenter;
        private CloudDataPresenter _cloudSavePresenter;
        private IResourceLoaderService _resourceLoaderService;

        public async UniTask Initialize(IInstanceLoader instanceLoader, IResourceLoaderService resourceLoaderService, GameObject saveModeUIPrefab, RectTransform parentForUI, params SaveStrategy[] saveStrategies)
        {
            _saveStrategies = saveStrategies;
            _saveModeUIPrefab = saveModeUIPrefab;
            _resourceLoaderService = resourceLoaderService;
            _parentForUI = parentForUI;
            _cloudSavePresenter = _saveStrategies.FirstOrDefault((strategy) => strategy is CloudDataPresenter) as CloudDataPresenter;
            _localSavePresenter = _saveStrategies.FirstOrDefault((strategy) => strategy is LocalSaveStrategyPresenter) as LocalSaveStrategyPresenter;
            base.Initialize(instanceLoader);

            if (!_initialized)
            {
                await DefineStrategy();
                _initialized = true;
                OnInternetConnected += TryOpenWindowSaveMode;
                OnInternetConnected += DefineStrategy;
                OnInternetDisconnected += DefineStrategy;
            }

            else
            {
                _currentSaveStrategy.UpdateAllDataUI();
            }
        }

        private UniTask TrySynchronizeData(SaveStrategy otherStrategy)
        {
            DataSave synchronizeData = _instanceLoader.CreateInstance<DataSave>();

            if (IsConnected && _currentSaveStrategy.LastSaveTime < otherStrategy.LastSaveTime)
            {
                synchronizeData[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = otherStrategy.CountDeadEnemies;
                synchronizeData[KeyData.COINS_COUNT] = otherStrategy.CountCoins;
                synchronizeData[KeyData.ADS_DISABLED] = otherStrategy.NoAdsStatus;
                synchronizeData[KeyData.LAST_SAVE_TIME] = otherStrategy.LastSaveTime;
                return _currentSaveStrategy.UpdateAllData(synchronizeData);
            }
            else if(IsConnected && _currentSaveStrategy.LastSaveTime > otherStrategy.LastSaveTime)
            {
                synchronizeData[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = _currentSaveStrategy.CountDeadEnemies;
                synchronizeData[KeyData.COINS_COUNT] = CountCoins;
                synchronizeData[KeyData.ADS_DISABLED] = NoAdsStatus;
                synchronizeData[KeyData.LAST_SAVE_TIME] = _currentSaveStrategy.LastSaveTime;
                return otherStrategy.UpdateAllData(synchronizeData);
            }
            return UniTask.CompletedTask;
        }

        public async UniTask UpdateCoins(int countCoins)
        {
            await _currentSaveStrategy.AddCountCoins(countCoins);
            _currentSaveStrategy.UpdateUICountCoins(countCoins);
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

        public new void Dispose()
        {
            base.Dispose();
            OnInternetConnected -= TryOpenWindowSaveMode;
            OnInternetConnected -= DefineStrategy;
            OnInternetDisconnected -= DefineStrategy;
        }

        private async UniTask DefineStrategy(SaveChoice newSaveChoice = SaveChoice.NoChoice)
        {
            _saveModeChoice = newSaveChoice;

             await IsConnectionAvailable();

            if (newSaveChoice.Equals(SaveChoice.UseCloud) && IsConnected)
            {
                _currentSaveStrategy = _cloudSavePresenter;
                WaitForDisconnection();
                await TrySynchronizeData(_localSavePresenter);
            }

            else if (newSaveChoice.Equals(SaveChoice.UseLocal))
            {
                _currentSaveStrategy = _localSavePresenter;
                WaitForConnection();
                await TrySynchronizeData(_cloudSavePresenter);
            }

            else if (IsConnected)
            {
                _currentSaveStrategy = _cloudSavePresenter;
                WaitForDisconnection();
                await TrySynchronizeData(_localSavePresenter);

            }

            else
            {
                _currentSaveStrategy = _localSavePresenter;
                WaitForConnection();
                await TrySynchronizeData(_cloudSavePresenter);

            }

            _currentSaveStrategy.UpdateAllDataUI();
        }

        private async UniTask TryOpenWindowSaveMode()
        {
            if (!ChoiceIsMade)
            {
                GameObject saveModeUIGameObject = await _resourceLoaderService.InstantiateAsync(_saveModeUIPrefab, _parentForUI);
                _saveModeUI = saveModeUIGameObject.GetComponent<SaveModeUI>();
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
            }
        }
    }
}