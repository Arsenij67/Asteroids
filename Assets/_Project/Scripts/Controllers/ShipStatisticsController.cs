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
        private ShipStatisticsView _shipStatisticView;
        private IResourceLoaderService _resourceLoaderService;

        public void Initialize(ShipStatisticsView shipStatisticView, ShipStatisticsModel shipStatisticModel, IResourceLoaderService resourceLoaderService)
        {
            _shipStatisticView = shipStatisticView;
            _shipStatisticModel = shipStatisticModel;
            _resourceLoaderService = resourceLoaderService;
        }

        public void Initialize()
        {
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
        }

        public void UpdateDestroyedEnemies()
        {
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
            UpdateDataSave();
        }

        public void IncreaseCountLaserShoots()
        {
            _shipStatisticModel.CountShootsLaser++;
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
            DataSave playerSave = _resourceLoaderService.CreateInstance<DataSave>();
            playerSave.EnemiesDestroyed = _shipStatisticModel.EnemiesDestroyed;
            string jsonData = JsonUtility.ToJson(playerSave);
            PlayerPrefs.SetString("statsSave", jsonData);
        }
    }
}
