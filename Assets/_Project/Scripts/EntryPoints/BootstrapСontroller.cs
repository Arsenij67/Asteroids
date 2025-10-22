using Asteroid.Database;
using Asteroid.Services.Analytics;
using Asteroid.Services.IAP;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapController : IInitializable, IDisposable
    {
        [Inject] private BootstrapUI _bootstrapUI;
        [Inject] private List<UniTask> _loadingTasks;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IAnalytics _analytics;
        [Inject] private IAdvertisementService _advertisementService;
        [Inject] private IRemoteConfigService  _remoteConfigService;
        [Inject] private BootstrapSceneData _bootstrapSceneModel;
        [Inject] private ShopSceneData _shopData;
        [Inject] private IPurchasingService _purchasingService;
        [Inject] private DataSave _dataForSave;

        private bool _analyticsReady;
        private bool _remoteConfigReady;
        private bool _sceneLoaded;
        private bool _advertisementReady;
        private float _loadingProgress;
        private bool _purchaseLoaded;
        private bool _shopLoaded;

        public async void Initialize()
        {
             await _sceneLoader.ReloadSceneAsync(_bootstrapSceneModel.SceneName);
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedGameScene;

            _loadingTasks.Add(PrepareAdvertisementAsync());
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareShopSceneAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(PrepareRemoteConfigAsync());
            _loadingTasks.Add(PreparePurchasingAsync());
            TickLoading();
             await UniTask.WhenAll(_loadingTasks);
            
            if (_bootstrapUI != null)
            {
                _bootstrapUI.ActivateButtonStart();
            }
        }

        private async UniTask PrepareShopSceneAsync()
        {
             await _sceneLoader.LoadSceneAdditiveAsync(_shopData.SceneName,false);
            _shopLoaded = true;
        }

        private async void OpenSceneShop()
        {
            await _sceneLoader.SwitchSceneActivation(_shopData.SceneName, true);
        }

        public void Dispose()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= OpenLoadedGameScene;
        }

        public void SetUpUI(RectTransform parent)
        {
            if (_bootstrapUI.TryGetComponent<RectTransform>(out RectTransform rectTransform))
            {
                rectTransform.SetParent(parent, false);
            }

        }

        private float UpdateProgress()
        {
            int completedCount = 0;
            completedCount += Convert.ToInt16(_analyticsReady);
            completedCount += Convert.ToInt16(_sceneLoaded);
            completedCount += Convert.ToInt16(_advertisementReady);
            completedCount += Convert.ToInt16(_remoteConfigReady);
            completedCount += Convert.ToInt16(_purchaseLoaded);
            completedCount += Convert.ToInt16(_shopLoaded);
            return _loadingProgress = (float)completedCount / _loadingTasks.Count();
        }

        private async UniTask PrepareAdvertisementAsync()
        {
            _advertisementService.Initialize(false,_dataForSave);
            await UniTask.CompletedTask;
            _advertisementReady = true;
        }

        private async UniTask PrepareAnalyticsAsync()
        {
            await _analytics.Initialize();
            _analyticsReady = true;
        }

        private async void TickLoading()
        {
            const float TICK_TIME = 0.2f;
            while (_loadingProgress < _bootstrapSceneModel.finalProgressTasks)
            {
                UpdateProgress();
                if (_bootstrapUI != null)
                {
                    UpdateBootstrapUI();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME));
            }
            OpenSceneShop();
        }

        private async UniTask PrepareGameSceneAsync()
        {
            await _sceneLoader.LoadSceneAsync(_bootstrapSceneModel.SceneGame,false);
            _sceneLoaded = true;
        }

        private async UniTask PrepareRemoteConfigAsync()
        {
            await _remoteConfigService.Initialize();
            _remoteConfigReady = true;
        }

        private async UniTask PreparePurchasingAsync()
        { 
            await _purchasingService.Initialize(_dataForSave);
            _purchaseLoaded = true;
        }

        private void UpdateBootstrapUI()
        {
            if (_bootstrapUI != null)
            {
                _bootstrapUI.UpdateSlider(_loadingProgress);
            }
        }

        private async void OpenLoadedGameScene()
        {
            Debug.Log(" open " + _bootstrapSceneModel.SceneGame);
            await _sceneLoader.SwitchSceneActivation(_bootstrapSceneModel.SceneGame, true);
            await _sceneLoader.UnloadSceneAsync(_bootstrapSceneModel.SceneName);
            Debug.Log(" end open " + _bootstrapSceneModel.SceneGame);

        }
    }
}