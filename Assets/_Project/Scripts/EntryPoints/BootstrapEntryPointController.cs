using System;
using System.Collections;
using Cysharp.Threading.Tasks;
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

        private async void Start()
        {
            _resourceLoader = new BaseResourceLoaderService();
            _sceneGameLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _sceneBootstrapLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _sceneUnloader = _resourceLoader.CreateInstance<SimpleSceneUnloader>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _bootstrapUI = GetComponent<BootstrapUI>();
            _sceneBootstrapLoader.ReloadScene();
            _bootstrapUI.OnPlayerClickButtonStart += OpenLoadedScene;
            TickLoading();
            await PrepareGameSceneAsync();
            await UniTask.WaitForSeconds(_bootstrapSceneModel.timeWaitLoading);
            _bootstrapUI.ActivateButtonStart();
        }

        private void OnDestroy()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= OpenLoadedScene;
        }

        private UniTask PrepareGameSceneAsync()
        {
            return _sceneGameLoader.LoadSceneAsyncAdditive(_bootstrapSceneModel.ScenePreLoad,false);
        }

        private async void TickLoading()
        {
            const float TICK_TIME = 0.2f;
            while (_sceneGameLoader.LoadingProgress < _bootstrapSceneModel.finalLoadingShare)
            {
               await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME));
               UpdateBootstrapUI();
            }
        }

        private void UpdateBootstrapUI()
        {
            _bootstrapUI.UpdateSlider(_sceneGameLoader.LoadingProgress);
        }

        private async void OpenLoadedScene()
        {
            _sceneGameLoader.SwitchSceneActivation(true);
            await UniTask.WaitUntil(() => _sceneGameLoader.LoadingProgress >= _bootstrapSceneModel.finalLoadingShare);
            await _sceneUnloader.UnloadSceneAsync(_bootstrapSceneModel.BootstrapSceneName);
            
        }
    }
}