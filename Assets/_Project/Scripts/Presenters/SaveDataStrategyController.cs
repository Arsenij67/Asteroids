
using Asteroid.Database.Connection;
using Asteroid.Generation;
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
        private SaveStrategy[] _saveStrategies;
        private SaveStrategy _currentSaveStrategy;
        private GameObject _saveModeUIPrefab;
        private RectTransform _parentForUI;
        private SaveModeUI? _saveModeUI;
        private IResourceLoaderService _resourceLoaderService;
        //public event Action<SaveChoice> OnStrategyChanged;
        //public event Action<DateTime> OnLastSaveTimeUpdated;

        //public SaveChoice CurrentStrategyType => _currentSaveStrategy?.GetMode() ?? SaveChoice.UseLocal;
        //public bool IsOnline => _isOnline;

        public async UniTask Initialize(IInstanceLoader instanceLoader, IResourceLoaderService resourceLoaderService, GameObject saveModeUIPrefab, RectTransform parentForUI, params SaveStrategy[] saveStrategies)
        {
            _saveStrategies = saveStrategies;
            _saveModeUIPrefab = saveModeUIPrefab;
            _resourceLoaderService = resourceLoaderService;
            _parentForUI = parentForUI;
            _saveModeChoice = SaveChoice.NoChoice;
            base.Initialize(instanceLoader);
            OnInternetConnected += OpenWindowSaveMode;
            OnInternetDisconnected += DefineStrategy;
            await DefineStrategy();
        }

        private async UniTask DefineStrategy()
        {
            IsConnected = await IsConnectionAvailable();
            if (IsConnected)
            {
                _currentSaveStrategy = _saveStrategies[0];
          
                Debug.Log($"Стратегия уже {_currentSaveStrategy.GetMode()}");
                WaitForDisconnection();
            }

            else
            {
                _currentSaveStrategy = _saveStrategies[1];
             
                Debug.Log($"Стратегия изменена на: {_currentSaveStrategy.GetMode()}");
                WaitForConnection();
            }
        }

        private async void DefineStrategy(SaveChoice newSaveChoice)
        {
            _saveModeChoice = newSaveChoice;
            IsConnected = await IsConnectionAvailable();

            if (newSaveChoice.Equals(SaveChoice.UseCloud) && IsConnected)
            {
                _currentSaveStrategy = _saveStrategies[0];
                await WaitForDisconnection();
            }

            else if (newSaveChoice.Equals(SaveChoice.UseLocal))
            {
                _currentSaveStrategy = _saveStrategies[1];
                await WaitForConnection();
            }
            Debug.Log($"Стратегия : {_currentSaveStrategy.GetMode()}");
        }

        private async UniTask OpenWindowSaveMode()
        {
            if (!ChoiceIsMade)
            {
                GameObject saveModeUIGameObject = await _resourceLoaderService.InstantiateAsync(_saveModeUIPrefab, _parentForUI);
                _saveModeUI = saveModeUIGameObject.GetComponent<SaveModeUI>();
                _saveModeUI.Initialize();
                _saveModeUI.OnButtonClosePressed += CloseWindowSaveMode;
                _saveModeUI.OnActiveItemChanged += DefineStrategy;
            }

            else
            { 
                DefineStrategy(_saveModeChoice);
            }
        }

        private void CloseWindowSaveMode()
        {
            if (ChoiceIsMade)
            {
                _saveModeUI.OnButtonClosePressed -= CloseWindowSaveMode;
                _saveModeUI.OnActiveItemChanged -= DefineStrategy;
                _saveModeUI?.CloseWindow();
            }
        }

        public async void UpdateCoinsAfterPurchase(int countCoins)
        {
            await _currentSaveStrategy.AddCountCoins(countCoins);
        }

        public async void UpdateNoAdsAfterPurchase(bool isCanceled)
        {
            await _currentSaveStrategy.UpdateNoAdsStatus(isCanceled);
        }

        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            _currentSaveStrategy.UpdateUINoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            _currentSaveStrategy.UpdateUICountCoins(countToAdd);
        }

        public new void Dispose()
        {
            base.Dispose();
            OnInternetConnected -= OpenWindowSaveMode;
            OnInternetDisconnected -= DefineStrategy;
        }

    }
}