using Asteroid.Database;
using Asteroid.Database.Connection;
using Asteroid.Exit;
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
    public class BootstrapEntryPoint : MonoBehaviour
    {
        [SerializeField] private SaveModeUI _saveModeUI;
        [SerializeField] private BootstrapUI _bootstrapUI;

        [Inject] private List<UniTask> _loadingTasks;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IAnalytics _analytics;
        [Inject] private IAdvertisementService _advertisementService;
        [Inject] private IRemoteConfigService _remoteConfigService;
        [Inject] private BootstrapSceneData _bootstrapSceneModel;
        [Inject] private ShopSceneData _shopData;
        [Inject] private IPurchasingService _purchasingService;
        [Inject] private DataSave _dataForSave;
        [Inject] private IApplicationQuitter _applicationQuitter;
        [Inject] private IRemoteSavable _remoteSave;
        [Inject] private WIFIConnector  _WIFIConnector;

        private bool _analyticsReady;
        private bool _remoteConfigReady;
        private bool _sceneLoaded;
        private bool _advertisementReady;
        private float _loadingProgress;
        private bool _purchaseLoaded;
        private bool _cloudSaveLoaded;
        private bool _shopLoaded;

        public async void Awake()
        {
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedGameScene;
            _bootstrapUI.OnPlayerClickButtonExit += _applicationQuitter.Quit;
            _bootstrapUI.Initialize();
            await _sceneLoader.ReloadSceneAsync(_bootstrapSceneModel.StartSceneName);
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedGameScene;
            _loadingTasks.Add(PrepareAdvertisementAsync());
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareShopSceneAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(PrepareRemoteConfigAsync());
            _loadingTasks.Add(PreparePurchasingAsync());
            _loadingTasks.Add(PrepareCloudSaveServiceAsync());
            TickLoading();
            await UniTask.WhenAll(_loadingTasks);
            _bootstrapUI.ActivateButtonStart();
        }

        private void OnDestroy()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= OpenLoadedGameScene;
            _bootstrapUI.OnPlayerClickButtonExit -= _applicationQuitter.Quit;
            _bootstrapUI.Dispose();
            _sceneLoader.UnloadScene(_shopData.StartSceneName);
        }

        private async UniTask PrepareShopSceneAsync()
        {
             await _sceneLoader.LoadSceneAdditiveAsync(_shopData.StartSceneName,false);
            _shopLoaded = true;
        }

        private UniTask OpenSceneShop()
        {
           return _sceneLoader.SwitchSceneActivation(_shopData.StartSceneName, true);
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
            completedCount += Convert.ToInt16(_cloudSaveLoaded);
            return _loadingProgress = (float)completedCount / _loadingTasks.Count();
        }

        private async UniTask PrepareAdvertisementAsync()
        {
            await _advertisementService.Initialize(false,_dataForSave);
            _advertisementReady = true;
        }

        private async UniTask PrepareAnalyticsAsync()
        {
            await _analytics.Initialize();
            _analyticsReady = true;
        }

        private async UniTask TickLoading()
        {
            const float TICK_TIME = 0.2f;
            while (_loadingProgress < _bootstrapSceneModel.finalProgressTasks)
            {
                UpdateProgress();
                UpdateBootstrapUI();
              
                await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME));
            }
            await OpenSceneShop();
        }

        private async UniTask PrepareGameSceneAsync()
        {
            await _sceneLoader.LoadSceneAsync(_bootstrapSceneModel.SceneGameName,false);
            _sceneLoaded = true;
        }

        private async UniTask PrepareRemoteConfigAsync()
        {
            await _remoteConfigService.Initialize();
            _remoteConfigReady = true;
        }

        private async UniTask PreparePurchasingAsync()
        { 
            await _purchasingService.Initialize();
            _purchaseLoaded = true;
        }

        private void UpdateBootstrapUI()
        {
            _bootstrapUI.UpdateSlider(_loadingProgress);
        }

        private async UniTask PrepareCloudSaveServiceAsync()
        { 
            await _remoteSave.Initialize(_dataForSave,_WIFIConnector);
            _cloudSaveLoaded = true;
        }

        private async void OpenLoadedGameScene()
        {
            await _sceneLoader.SwitchSceneActivation(_bootstrapSceneModel.SceneGameName, true);
            await _sceneLoader.UnloadSceneAsync(_bootstrapSceneModel.StartSceneName);
        }
    }
}