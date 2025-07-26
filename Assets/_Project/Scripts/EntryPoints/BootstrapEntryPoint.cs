using Asteroid.Services;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapEntryPointController : MonoBehaviour
    {
        public event Action OnGameStarted;
        [SerializeField] private RectTransform _userInterface;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IInstanceLoader _instanceLoader;
        private BootstrapSceneModel _bootstrapSceneModel;
        [Inject] private BootstrapUI _bootstrapUI;
        [Inject] private IResourceLoaderService _resourceLoader;
        private IAnalytics _analytics;
        private List<UniTask> _loadingTasks;
        private bool _analyticsReady;
        private bool _sceneLoaded;
        private bool _waitingCompleted;
        private float _loadingProgress;

        
        private object t;
        public object _loadedScene { get { return t;  } set { Debug.Log("changed on" + value); t = value; } }


        private void Awake()
        {
            _loadingTasks = _instanceLoader.CreateInstance<List<UniTask>>();
            _analytics = _instanceLoader.CreateInstance<FirebaseAnalyticsSender>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
        }

        private async void Start()
        {
            SetUpUI(_userInterface);
            _loadedScene =  await _sceneLoader.ReloadSceneAsync(_bootstrapSceneModel.BootstrapSceneName);            
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

        private void Update()
        {
            Debug.Log((_loadedScene != null) ? " ls != null " + ((AsyncOperationHandle<SceneInstance>)_loadedScene).Result.Scene.name : "False  - "+ _loadedScene);
            Debug.Log(_sceneLoader.LastLoadedScene+" last loaded");
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

        private void SetUpSceneData(object newData)
        {
            if (newData!=null)
            {
                _loadedScene = newData;
            }
        }

        private void SetUpUI(RectTransform parent)
        {
            if (_bootstrapUI.TryGetComponent<RectTransform>(out RectTransform rectTransform))
            {
                rectTransform.SetParent(parent, false);
            }

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
            Debug.Log(" PrepareGameSceneAsync");
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
            Debug.Log((_loadedScene != null) ? " ls != null " 
                + ((AsyncOperationHandle<SceneInstance>)_loadedScene).Result.Scene.name :
                "False");
            await ((LocalBundleSceneLoader)_sceneLoader).SwitchSceneActivation(true,0);
            await _sceneLoader.UnloadSceneAsync(_loadedScene);

        }
    }
}