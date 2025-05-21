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
            _shipStatisticView.EnableRestartAction(ReloadScene);
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
        }

        public void RemoveAllListeners()
        {
            _shipStatisticView.DisableRestartAction(ReloadScene);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void UpdateDestroyedEnemies()
        {
            _shipStatisticView.UpdateDestroyedEnemies(_shipStatisticModel.EnemiesDestroyed);
        }
    }
}
