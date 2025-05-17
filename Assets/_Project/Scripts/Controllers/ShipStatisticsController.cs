using Asteroid.Statistic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asteroid.SpaceShip
{
    public class ShipStatisticsController
    {
        public ShipStatisticsModel ShipStatisticModel { get; private set;}
        public ShipStatisticsView ShipStatisticView { get; private set; }
        public void Initialize(ShipStatisticsView shipStatisticView, ShipStatisticsModel shipStatisticModel)
        {
            ShipStatisticView = shipStatisticView;
            ShipStatisticModel = shipStatisticModel;
        }
        public void Initialize()
        { 
            ShipStatisticView.EnableRestartAction(ReloadScene);
            ShipStatisticView.UpdateDestroyedEnemies(ShipStatisticModel.EnemiesDestroyed);
        }
        public void RemoveAllListeners()
        {
            ShipStatisticView.DisableRestartAction(ReloadScene);
        }
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void UpdateDestroyedEnemies()
        {
            ShipStatisticView.UpdateDestroyedEnemies(ShipStatisticModel.EnemiesDestroyed);
        }
    }
}
