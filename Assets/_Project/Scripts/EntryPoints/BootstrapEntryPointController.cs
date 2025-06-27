using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Asteroid.Services;

namespace Asteroid.Generation
{
    [RequireComponent(typeof(BootstrapUI))]
    public class BootstrapEntryPointController : MonoBehaviour
    {
        public event Action OnGameStarted;

        private BootstrapSceneModel _bootstrapSceneModel;
        private BootstrapUI _bootstrapUI;
        private ISceneLoader _sceneBootstrapLoader;
        private ISceneLoader _sceneGameLoader;
        private ISceneUnloader _sceneUnloader;
        private IResourceLoaderService _resourceLoader;
        private IInstanceLoader _instanceLoader;
        private IAnalytics _analytics;
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;

        private void Awake()
        {
            _instanceLoader = new InstanceCreator();
            _resourceLoader = _instanceLoader.CreateInstance<BaseResourceLoaderService>();
            _sceneGameLoader = _instanceLoader.CreateInstance<SimpleSceneLoader>();
            _loadingTasks = _instanceLoader.CreateInstance<List<UniTask>>();
            _sceneBootstrapLoader = _instanceLoader.CreateInstance<SimpleSceneLoader>();
            _sceneUnloader = _instanceLoader.CreateInstance<SimpleSceneUnloader>();
            _analytics = _instanceLoader.CreateInstance<FirebaseAnalyticsSender>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _bootstrapUI = GetComponent<BootstrapUI>();
        }

        private async void Start()
        {
            _sceneBootstrapLoader.ReloadScene();
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedScene;
            _bootstrapUI.OnPlayerClickButtonStart += () => OnGameStarted?.Invoke();
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(WaitBeforeLoadingSceneAsync(_bootstrapSceneModel.timeWaitLoading));
            TickLoading();
            await UniTask.WhenAll(_loadingTasks);
            _bootstrapUI.ActivateButtonStart();
        }

        private void OnDestroy()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= OpenLoadedScene;
        }
        private void UpdateProgress()
        {
            int completedCount = 0;
            completedCount += Convert.ToInt32(_analyticsReady);
            completedCount += Convert.ToInt32(_sceneLoaded);
            completedCount += Convert.ToInt32(_waitingCompleted);
            _loadingProgress = (float)completedCount / _loadingTasks.Count();
        }

        private async UniTask PrepareAnalyticsAsync()
        {
            _analyticsReady = await _analytics.Initialize();
        }
        private async void TickLoading()
        {
            const float TICK_TIME = 0.2f;
            while (_loadingProgress < _bootstrapSceneModel.finalProgressTasks)
            {
                UpdateProgress();
                UpdateBootstrapUI();
                await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        private async UniTask PrepareGameSceneAsync()
        {
            await _sceneGameLoader.LoadSceneAsyncAdditive(_bootstrapSceneModel.ScenePreLoad, false);
            _sceneLoaded = true;
        }

        private async UniTask WaitBeforeLoadingSceneAsync(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            _waitingCompleted = true;
        }

        private void UpdateBootstrapUI()
        {
            _bootstrapUI.UpdateSlider(_loadingProgress);
        }

        private async void OpenLoadedScene()
        {
            _sceneGameLoader.SwitchSceneActivation(true);
            await UniTask.WaitUntil(() => _sceneGameLoader.LoadingProgress >= _bootstrapSceneModel.finalLoadingShare);
            await _sceneUnloader.UnloadSceneAsync(_bootstrapSceneModel.BootstrapSceneName);
        }
    }
}