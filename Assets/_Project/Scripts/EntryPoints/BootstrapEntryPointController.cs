using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Asteroid.Services;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

namespace Asteroid.Generation
{
    [RequireComponent(typeof(BootstrapUI))]
    public class BootstrapEntryPointController : MonoBehaviour
    {
        public event Action OnGameStarted;

        private BootstrapSceneModel _bootstrapSceneModel;
        private BootstrapUI _bootstrapUI;
        [Inject] private ISceneLoader _bootstrapSceneLoader;
        [Inject] private ISceneLoader _gameSceneLoader;
        private IResourceLoaderService _resourceLoader;
        [Inject] private IInstanceLoader _instanceLoader;
        private IAnalytics _analytics;
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;


        private void Awake()
        {
            _resourceLoader = _instanceLoader.CreateInstance<BaseResourceLoaderService>();
            _loadingTasks = _instanceLoader.CreateInstance<List<UniTask>>();
            _analytics = _instanceLoader.CreateInstance<FirebaseAnalyticsSender>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _bootstrapUI = GetComponent<BootstrapUI>();
        }

        private async void Start()
        {
            _bootstrapSceneLoader.ReloadScene(_bootstrapSceneModel.BootstrapSceneName);
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
                if (this == null)
                    return;
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
            Debug.Log("PrepareGameSceneAsync()");
            await _gameSceneLoader.LoadSceneAdditiveAsync(_bootstrapSceneModel.ScenePreLoad,false);
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
            Debug.Log(9);
            _gameSceneLoader.SwitchSceneActivation(true);
            Debug.Log(9);


            await _bootstrapSceneLoader.UnloadSceneAsync();
            Debug.Log(9);
        }

        public void OOO()
        {
           _gameSceneLoader.UnloadSceneAsync();
        }
    }
}