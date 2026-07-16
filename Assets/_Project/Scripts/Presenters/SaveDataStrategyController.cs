
using Asteroid.Database.Connection;
using Asteroid.Generation;
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

        private SaveStrategy[] _saveStrategies;
        private SaveStrategy _currentSaveStrategy;

        //public event Action<SaveChoice> OnStrategyChanged;
        //public event Action<DateTime> OnLastSaveTimeUpdated;

        //public SaveChoice CurrentStrategyType => _currentSaveStrategy?.GetMode() ?? SaveChoice.UseLocal;
        //public bool IsOnline => _isOnline;

        public async UniTask Initialize(IInstanceLoader instanceLoader, params SaveStrategy[] saveStrategies)
        {
            _saveStrategies = saveStrategies;
            base.Initialize(instanceLoader);   
            await DefineStrategy(SaveChoice.UseCloud);
        }

        private async UniTask DefineStrategy(SaveChoice saveChoice)
        {
            _currentSaveStrategy = _saveStrategies[0];

            if (await IsConnectionAvailable())
            {
                Debug.Log($"Стратегия уже {_currentSaveStrategy.GetMode()}, изменений не требуется");
                return;
            }

            else
            {
                OnInternetConnected += SuggestChangingSaveModeWindow;
                _currentSaveStrategy = _saveStrategies[1];
                Debug.Log($"Стратегия изменена на: {_currentSaveStrategy.GetMode()}");
     
            }
         
        }

        private void SuggestChangingSaveModeWindow()
        {
            Debug.Log("Появился интернет. Хотите сменить режим сохранения?");
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
            OnInternetConnected -= SuggestChangingSaveModeWindow;
            base.Dispose();
   
        }
    }
}