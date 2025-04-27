using Asteroid.Statistic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(ShipStatisticsModel))]
    public class ShipStatisticsController : MonoBehaviour
    {
        [SerializeField] private ShipStatisticsView _shipStView;

        private ShipStatisticsModel _shipStModel;
        private void Awake()
        {
            _shipStModel = GetComponent<ShipStatisticsModel>();
            _shipStView.AddRestartAction(ReloadScene);
        }
        private void OnDestroy()
        {
            _shipStView.DisableRestartAction(ReloadScene);
        }
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void UpdateDestroyedEnemies()
        {
            _shipStView.UpdateDestroyedEnemies(_shipStModel._enemiesDestroyed);
        }
    }
}
