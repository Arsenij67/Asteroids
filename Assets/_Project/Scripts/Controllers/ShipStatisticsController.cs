using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Statistic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asteroid.SpaceShip
{
    public class ShipStatisticsController
    {
        private ShipStatisticsModel _shipStatisticModel;
        private GameOverView _gameOverView;
        private IInstanceLoader _resourceLoaderService;
        private DataSave _playerSave;

        public void Initialize(GameOverView gameOverView, ShipStatisticsModel shipStatisticModel, IInstanceLoader instanceLoader, DataSave dataSave)
        {
            _gameOverView = gameOverView;
            _shipStatisticModel = shipStatisticModel;
            _resourceLoaderService = instanceLoader;
            _playerSave = dataSave;
        }

        public void UpdateDestroyedEnemies()
        {
            _gameOverView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
            UpdateDataSave();
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
        private void UpdateDataSave()
        {
            _playerSave.EnemiesDestroyed = _shipStatisticModel.EnemiesDestroyed;
            string jsonData = JsonUtility.ToJson(_playerSave);
            PlayerPrefs.SetString("statsSave", jsonData);
        }
    }
}
