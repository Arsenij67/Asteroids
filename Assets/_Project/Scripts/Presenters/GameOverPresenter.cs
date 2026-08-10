using Asteroid.Generation;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.Statistic;
using System;
using UnityEngine;

namespace Asteroid.SpaceShip
{
    public class GameOverPresenter : IDisposable
    {
        private ISceneLoader _sceneLoader;
        private IResourceLoader _resourceLoader;
        private GameOverView? _gameOverView;
        private GameOverView _endPanelPrefab;
        private ShipStatisticsModel _shipStatisticModel;
        private AdvertisementPresenter _advertisementPresenter;
        private BootstrapSceneData _bootstrapSceneData;
        private RectTransform _parentForAttachment;

        public void Dispose()
        {
            _gameOverView.OnGameReloadClicked -= _sceneLoader.ReloadCurrentScene;
            _gameOverView.OnButtonGoHomeClicked -= LoadMainMenuScene;
            _gameOverView.OnButtonShowAdsClicked -= _advertisementPresenter.ShowRewardedAdAfterDead;
            _gameOverView.OnGameReloadClicked -= _advertisementPresenter.ShowInterstitialAdBeforeRestart;
            _advertisementPresenter.OnPlayerRevived -= ClosePanelRestart;
        }

        public void Initialize (IResourceLoader resourceLoader,RectTransform parentForAttachment, GameOverView endPanelPrefab, AdvertisementPresenter advertisementPresenter, ShipStatisticsModel shipStatisticsModel,RectTransform parentForEndWindow, ISceneLoader sceneLoader, BootstrapSceneData bootstrapSceneData)
        {
            _shipStatisticModel = shipStatisticsModel; 
            _advertisementPresenter = advertisementPresenter;
            _sceneLoader = sceneLoader;
            _bootstrapSceneData = bootstrapSceneData;
            _endPanelPrefab = endPanelPrefab;
            _parentForAttachment = parentForAttachment;
            _resourceLoader = resourceLoader;
        }

        public void OpenPanelRestart()
        {
            _gameOverView = CreateGameOverWindow(_endPanelPrefab, _parentForAttachment);
            _gameOverView.OnGameReloadClicked += _sceneLoader.ReloadCurrentScene;
            _gameOverView.OnButtonGoHomeClicked += LoadMainMenuScene;
            _gameOverView.OnButtonShowAdsClicked +=  _advertisementPresenter.ShowRewardedAdAfterDead;
            _gameOverView.OnGameReloadClicked +=  _advertisementPresenter.ShowInterstitialAdBeforeRestart;
            _advertisementPresenter.OnPlayerRevived += ClosePanelRestart;
            _gameOverView.UpdateButtonShowAd(_advertisementPresenter.IsShowed,_advertisementPresenter.IsInitialized);
        }

        public void ClosePanelRestart()
        {
            _resourceLoader.UnloadResource(_gameOverView.name);
            _gameOverView.Close();
        }

        public void UpdateDestroyedEnemiesUI()
        {
            _gameOverView?.UpdateDestroyedEnemies(_shipStatisticModel.CountEnemiesDestroyed);
        }

        private void LoadMainMenuScene()
        {
            _sceneLoader.LoadScene(_bootstrapSceneData.StartSceneName);  
        }

        private GameOverView CreateGameOverWindow(GameOverView gameOverViewPrefab, RectTransform parentAttachment)
        {
            GameOverView gameOverView =  _resourceLoader.Instantiate(gameOverViewPrefab, parentAttachment);
            gameOverView.Initialize();
            return gameOverView;
        }
    }
}
