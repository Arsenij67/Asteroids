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
        private BootstrapSceneModel _bootstrapSceneModel;
        private BootstrapUI _bootstrapUI;
        private ISceneLoader _sceneBootstrapLoader;
        private ISceneLoader _sceneGameLoader;
        private ISceneUnloader _sceneUnloader;
        private IResourceLoaderService _resourceLoader;
        private IAnalytics _analytics;
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;

        private void Awake()
        {
            _resourceLoader = new BaseResourceLoaderService();
            _sceneGameLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _loadingTasks = _resourceLoader.CreateInstance<List<UniTask>>();   
            _sceneBootstrapLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _sceneUnloader = _resourceLoader.CreateInstance<SimpleSceneUnloader>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _analytics = _resourceLoader.CreateInstance<FirebaseAnalytics>();
            _bootstrapUI = GetComponent<BootstrapUI>();
        }

        private async void Start()
        {
            _sceneBootstrapLoader.ReloadScene();
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedScene;
            _bootstrapUI.OnPlayerClickButtonStart += SendEventGameStart;
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
            _bootstrapUI.OnPlayerClickButtonStart -= SendEventGameStart;
        }

        private void UpdateProgress()
        {
            int completedCount = 0;
            completedCount += Convert.ToInt32(_analyticsReady);
            completedCount += Convert.ToInt32(_sceneLoaded);
            completedCount += Convert.ToInt32(_waitingCompleted);
            _loadingProgress =(float) completedCount / _loadingTasks.Count();
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
            await UniTask.WaitForSeconds(seconds);
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

        private void SendEventGameStart()
        { 
            _analytics.PushEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, Firebase.Analytics.FirebaseAnalytics.ParameterStartDate, DateTime.UtcNow);
        }
    }
}