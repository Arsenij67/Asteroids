using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;

        private async void Start()
        {
            _resourceLoader = new BaseResourceLoaderService();
            _sceneGameLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _loadingTasks = _resourceLoader.CreateInstance<List<UniTask>>();   
            _sceneBootstrapLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _sceneUnloader = _resourceLoader.CreateInstance<SimpleSceneUnloader>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _bootstrapUI = GetComponent<BootstrapUI>();
            _sceneBootstrapLoader.ReloadScene();
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedScene;
            _loadingTasks.Add(PrepareAnalyticsAsync());
            _loadingTasks.Add(PrepareGameSceneAsync());
            _loadingTasks.Add(WaitBeforeLoadingSceneAsync(_bootstrapSceneModel.timeWaitLoading));
            TickLoading();
            await UniTask.WhenAll(_loadingTasks);
            _bootstrapUI.ActivateButtonStart();
            FirebaseAnalytics.LogEvent("custom_progress_event", "percent", 0.4f);
            FirebaseAnalytics.SetUserProperty("favorite_food", "ice cream");
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
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase initialized successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
            _analyticsReady = true;
        }
        private async void TickLoading()
        {
            const float TICK_TIME = 0.2f;
            while (_loadingProgress<1)
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

        private void OnDestroy()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= OpenLoadedScene;
        }
    }
}