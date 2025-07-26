using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Asteroid.Services;
using Asteroid.UI;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapEntryPoint : IInitializable, IDisposable
    {
        public event Action OnGameStarted;

        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IInstanceLoader _instanceLoader;
        [Inject] private BootstrapUI _bootstrapUI;
        private BootstrapSceneModel _bootstrapSceneModel;
        private IResourceLoaderService _resourceLoader;
        private IAnalytics _analytics;
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;
        private object _loadedScene;

        public async void Initialize()
        {
            _resourceLoader = _instanceLoader.CreateInstance<BaseResourceLoaderService>();
            _loadingTasks = _instanceLoader.CreateInstance<List<UniTask>>();
            _analytics = _instanceLoader.CreateInstance<FirebaseAnalyticsSender>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");

            _loadedScene =_sceneLoader.ReloadSceneAsync(_bootstrapSceneModel.BootstrapSceneName);
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedScene;
            _bootstrapUI.OnPlayerClickButtonStart += () => OnGameStarted?.Invoke();
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(WaitBeforeLoadingSceneAsync(_bootstrapSceneModel.timeWaitLoading));
            TickLoading();
            await UniTask.WhenAll(_loadingTasks);
            if (_bootstrapUI != null)
            {
                _bootstrapUI.ActivateButtonStart();
            }

        }

        public void Dispose()
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
                if (_bootstrapUI != null)
                {
                    UpdateBootstrapUI();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME));
            }
        }

        private async UniTask PrepareGameSceneAsync()
        {
            await _sceneLoader.LoadSceneAdditiveAsync(_bootstrapSceneModel.ScenePreLoad,false);
            _sceneLoaded = true;
        }

        private async UniTask WaitBeforeLoadingSceneAsync(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            _waitingCompleted = true;
        }

        private void UpdateBootstrapUI()
        {
            if (_bootstrapUI != null)
            {
                _bootstrapUI.UpdateSlider(_loadingProgress);
            }
        }

        private async void OpenLoadedScene()
        {
            _sceneLoader.SwitchSceneActivation(true);
            await _sceneLoader.UnloadSceneAsync(_loadedScene);

        }
    }
}