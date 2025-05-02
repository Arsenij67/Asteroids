using Asteroid.Statistic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroid.SpaceShip
{
    public class ShipStatisticsController
    {
        [field: SerializeField] public ShipStatisticsModel ShipStModel { get; private set;}
        public ShipStatisticsView ShipStView { get; private set; }
        private void OnDestroy()
        {
            ShipStView.DisableRestartAction(ReloadScene);
        }
        public void Init(ShipStatisticsView shipStView)
        {
            ShipStView = shipStView;
            ShipStView.EnableRestartAction(ReloadScene);
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
