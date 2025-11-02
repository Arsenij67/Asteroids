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
        private DataSave _playerSave;

        public void Initialize(ShipStatisticsModel shipStatisticModel, IInstanceLoader instanceLoader, DataSave dataSave)
        {
            _shipStatisticModel = shipStatisticModel;
            _playerSave = dataSave;
        }

        public void UpdateDestroyedEnemies(GameOverView gameOverView)
        {
            gameOverView.UpdateDestroyedEnemies(_shipStatisticModel.CountEnemiesDestroyed);
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
            _playerSave.CountSummaryEnemiesDestroyed = _shipStatisticModel.CountEnemiesDestroyed;
            string jsonData = JsonUtility.ToJson(_playerSave);
            PlayerPrefs.SetString("statsSave", jsonData);
        }
    }
}
