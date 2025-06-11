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

        public void Initialize(ShipStatisticsView shipStatisticView, ShipStatisticsModel shipStatisticModel)
        {
            _shipStatisticView = shipStatisticView;
            _shipStatisticModel = shipStatisticModel;
        }

        public void Initialize()
        {
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
        }

        public void UpdateDestroyedEnemies()
        {
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
        }
    }
}
