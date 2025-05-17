using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Asteroid.Statistic
{
    public class ShipStatisticsView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _fireballCountText;
        [SerializeField] private TMP_Text _laserCountText;
        [SerializeField] private TMP_Text _coordinatesText;
        [SerializeField] private TMP_Text _angleRotationText;
        [SerializeField] private TMP_Text _rollbackTimeText;
        [SerializeField] private TMP_Text _spaceShipVelocityText;
        
        private TMP_Text _enemiesDestroyedText;
        private Button _buttonRestart;
        public void Initialize(GameOverView gameOverView)
        {
            _enemiesDestroyedText = gameOverView.EnemiesDestroyedText;
            _buttonRestart = gameOverView.ButtonRestart;
        }
        public void UpdateFireballCount(int count)
        {
            if (_fireballCountText != null)
            {
                _fireballCountText.text = $"Fireballs: {count}";
            }
        }
        public void UpdateLaserCount(int count)
        {
            if (_laserCountText != null)
            {
                _laserCountText.text = $"Lasers: {count}";
            }
        }
        public void UpdateCoordinates(Vector3 position)
        {
            if (_coordinatesText != null)
            {
                _coordinatesText.text = $"Pos: {position.x:F1}, {position.y:F1}";
            }
        }
        public void UpdateAngleRotation(float angle)
        {
            if (_angleRotationText != null)
                _angleRotationText.text = $"Angle: {angle:F1}°";
        }
        public void UpdateRollbackTime(float time)
        {
            if (_rollbackTimeText != null)
            {
                _rollbackTimeText.text = $"Rollback: {time:F1}s";
            }
        }
        public void UpdateSpaceShipVelocity(Vector3 velocity)
        {
            if (_spaceShipVelocityText != null)
            {
                _spaceShipVelocityText.text = $"Speed: {velocity.magnitude:F1} m/s";
            }
        }
        public void UpdateDestroyedEnemies(int count)
        {
            if (_enemiesDestroyedText != null)
            {
                _enemiesDestroyedText.text = $"Enemies destroyed: {count:D1} units";
            }
        }
        public void EnableRestartAction(UnityAction actionRestart)
        {
            _buttonRestart.onClick.AddListener(actionRestart);
        }
        public void DisableRestartAction(UnityAction actionRestart)
        {
            _buttonRestart.onClick.RemoveListener(actionRestart);
        }
    }
}