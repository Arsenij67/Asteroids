using Asteroid.Generation;
using Asteroid.Statistic;
using System;
using UnityEngine;

namespace Asteroid.SpaceShip
{
    public class GameOverPresenter
    {
        private ShipStatisticsModel _shipStatisticModel;
        private IResourceLoaderService _resourceLoader;
        private GameOverView _gameOverView;
        private GameObject _endPanelPrefab;
        private RectTransform _parentForEndWindow;

        public void Initialize(ShipStatisticsModel shipStatisticModel, IResourceLoaderService resourceLoader, GameObject endPanelPrefab,RectTransform parentForEndWindow)
        {
            _shipStatisticModel = shipStatisticModel;
            _resourceLoader = resourceLoader;
            _endPanelPrefab = endPanelPrefab;
            _parentForEndWindow = parentForEndWindow;   
        }

        public void OpenPanelRestart()
        {
            _gameOverView = _resourceLoader.Instantiate(_endPanelPrefab, _parentForEndWindow).GetComponent<GameOverView>();
            _gameOverView.Initialize();



            _advertisingController.OnPlayerRevived += _endPanelView.Close;
            _endPanelView.OnGameReloadClicked += _sceneLoader.ReloadCurrentScene;
            _endPanelView.OnButtonGoHomeClicked += _obstaclesGenerationController.LoadMainMenuScene;
            _endPanelView.OnButtonShowAdsClicked += _advertisingController.ShowRewardedAdAfterDead;
            _endPanelView.OnGameReloadClicked += _advertisingController.ShowInterstitialAdBeforeRestart;
            _gameOverView.UpdateButtonShowAd(_advertisementService.IsShowed);
        }

        public void ClosePanelRestart()
        {
             OnPanelClosed?.Invoke();
            _gameOverView.Close();
        }

        public void UpdateDestroyedEnemiesUI()
        {
            _gameOverView?.UpdateDestroyedEnemies(_shipStatisticModel.CountEnemiesDestroyed);
        }

        public void UpdateButtonAdsUI(bool isShowed)
        {
            _gameOverView?.UpdateButtonShowAd(isShowed);
        }

        public void IncreaseCountLaserShoots()
        {
            _shipStatisticModel.CountShootsLaser++;
        }

        public void IncreaseCountBulletShoots()
        {
            _shipStatisticModel.CountShootsFireball++;
        }

        public void IncreaseCountUFODestroyed()
        {
            _shipStatisticModel.CountDestroyedUFO++;
        }

        public void IncreaseCountMeteoritesDestroyed()
        {
           _shipStatisticModel.CountDestroyedMeteorites++;
        }
        public void IncreaseCountAsteroidsDestroyed()
        {
            _shipStatisticModel.CountDestroyedAsteroids++;
        }
    }
}
