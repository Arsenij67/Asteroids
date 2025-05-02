using Asteroid.Statistic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asteroid.SpaceShip
{
    public class ShipStatisticsController
    {
        public ShipStatisticsModel ShipStModel { get; private set;}
        public ShipStatisticsView ShipStView { get; private set; }
        public void RemoveAllListeners()
        {
            ShipStView.DisableRestartAction(ReloadScene);
        }
        public void Init(ShipStatisticsView shipStView, ShipStatisticsModel shipStModel)
        {
            ShipStView = shipStView;
            ShipStModel = shipStModel;
        }
        public void Init()
        { 
            ShipStView.EnableRestartAction(ReloadScene);
            ShipStView.UpdateDestroyedEnemies(ShipStModel._enemiesDestroyed);
        }
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void UpdateDestroyedEnemies()
        {
            ShipStView.UpdateDestroyedEnemies(ShipStModel._enemiesDestroyed);
        }
    }
}
