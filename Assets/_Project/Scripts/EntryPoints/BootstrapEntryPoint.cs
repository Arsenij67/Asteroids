using Asteroid.Database;
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
using UnityEngine.UI;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapEntryPoint : MonoBehaviour, IDisposable
    {
        public event Action OnPlayerClickButtonStart;
        public event Action OnPlayerClickButtonExit;

        [SerializeField] private SaveModeUI _saveModeUI;
        [SerializeField] private Slider _sliderLoading;
        [SerializeField] private Button _buttonStartGame;
        [SerializeField] private Button _buttonExitGame;
        [SerializeField] private RectTransform _interfaceMount;

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
            _buttonStartGame.onClick.AddListener(NotifyButtonStartPressed);
            _buttonExitGame.onClick.AddListener(NotifyButtonExitPressed);

            await _sceneLoader.ReloadStartSceneAsync(_bootstrapSceneModel.StartSceneName);
            OnPlayerClickButtonStart += OpenLoadedGameScene;
            _loadingTasks.Add(PrepareAdvertisementAsync());
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareShopSceneAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(PrepareRemoteConfigAsync());
            _loadingTasks.Add(PreparePurchasingAsync());
            _loadingTasks.Add(PrepareCloudSaveServiceAsync());
            TickLoading();
            await UniTask.WhenAll(_loadingTasks);
            ActivateButtonStart();
            OnPlayerClickButtonExit += _applicationQuitter.Quit;
        }

        private void OnDestroy()
        {
            _buttonStartGame.onClick.RemoveListener(NotifyButtonStartPressed);
            _buttonExitGame.onClick.RemoveListener(NotifyButtonExitPressed);
            
        }

        public void Dispose()
        {
            OnPlayerClickButtonStart -= OpenLoadedGameScene;
            OnPlayerClickButtonExit -= _applicationQuitter.Quit;
        }

        public void SetUpUI(RectTransform parent)
        {
            _interfaceMount.SetParent(parent, false);
        }

        public void UpdateSlider(float endValue)
        {
            endValue = Mathf.Clamp01(endValue);
            _sliderLoading.value = endValue;
        }

        public void ActivateButtonStart()
        {
            if (_buttonStartGame != null)
            {
                _buttonStartGame.gameObject.SetActive(true);
            }
        }

        private void NotifyButtonStartPressed()
        {
            OnPlayerClickButtonStart?.Invoke();
        }

        private void NotifyButtonExitPressed()
        {
            OnPlayerClickButtonExit?.Invoke();
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
            await _purchasingService.Initialize(_dataForSave);
            _purchaseLoaded = true;
        }

        private void UpdateBootstrapUI()
        {
            UpdateSlider(_loadingProgress);
        }

        private async UniTask PrepareCloudSaveServiceAsync()
        { 
            await _remoteSave.Initialize(_dataForSave);
            _cloudSaveLoaded = true;
        }

        private async void OpenLoadedGameScene()
        {
            await _sceneLoader.SwitchSceneActivation(_bootstrapSceneModel.SceneGameName, true);
            await _sceneLoader.UnloadSceneAsync(_bootstrapSceneModel.StartSceneName);
        }
    }
}