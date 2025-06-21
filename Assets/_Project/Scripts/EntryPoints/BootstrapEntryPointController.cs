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
        private ISceneLoader _sceneLoader;
        private IResourceLoaderService _resourceLoader;

        private async void Start()
        {
            _resourceLoader = new BaseResourceLoaderService();
            _sceneLoader = _resourceLoader.CreateInstance<SimpleSceneLoader>();
            _bootstrapSceneModel = _resourceLoader.LoadResource<BootstrapSceneModel>("ScriptableObjects/BootstrapSceneData");
            _bootstrapUI = GetComponent<BootstrapUI>();
            _bootstrapUI.OnPlayerClickButtonStart += ActivateLoadedScene;

            TickLoading();
            await PrepareGameSceneAsync();
            await UniTask.WaitForSeconds(_bootstrapSceneModel.TIME_WAIT_LOADING);
            _bootstrapUI.ActivateButtonStart();
        }

        private void OnDestroy()
        {
            _bootstrapUI.OnPlayerClickButtonStart -= ActivateLoadedScene;
        }

        private UniTask PrepareGameSceneAsync()
        {
            return _sceneLoader.LoadSceneAsyncAdditive(_bootstrapSceneModel.PreLoadedScene,false);
        }

        private async void TickLoading()
        {
            const float TICK_TIME = 0.2f;
            const float FINAL_LOADING_SHARE = 0.9f;
            while (_sceneLoader.LoadingProgress < FINAL_LOADING_SHARE)
            {
               await UniTask.Delay(TimeSpan.FromSeconds(TICK_TIME));
               UpdateBootstrapUI();
            }
        }

        private void UpdateBootstrapUI()
        {
            _bootstrapUI.UpdateSlider(_sceneLoader.LoadingProgress);
        }

        private void ActivateLoadedScene()
        {
            _sceneLoader.SwitchSceneActivation(true);
        }
        
    }
}