using UnityEngine;
using Asteroid.Statistic;
using Asteroid.Enemies;
using Asteroid.Weapon;
using Asteroid.Inputs;
using Asteroid.SpaceObjectActions;
using System;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(SpaceShipData))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IDeviceInput))]
    public class SpaceShipController : SpaceObject
    {
        private event Func<ShipStatisticsController> OnGameOver;

        private IDeviceInput _deviceInput;
        private ShipStatisticsView _statisticsView;
        private ShipStatisticsController _statisticsController;

        private SpaceShipData _shipData;
        private Rigidbody2D _rbController;
        private void FixedUpdate()
        {
            TryTeleport(transform.position);
            TryRotateShip(_deviceInput.ScanRotation());
            TryMoveShip(_deviceInput.ScanMove());
        }
        public void Init(ShipStatisticsController stController)
        {
            _statisticsController = stController;
        }
        public void Init(Func<ShipStatisticsController> callBack, ShipStatisticsView stView)
        {
            OnGameOver = callBack;
            _shipData = GetComponent<SpaceShipData>();
            _rbController = GetComponent<Rigidbody2D>();
            _deviceInput = GetComponent<IDeviceInput>();
            _statisticsView = stView;
        }
        private bool TryRotateShip(float rotationInput)
        {
            if (Mathf.Approximately(rotationInput, 0f))
                return false;

            float rotationAngle = -rotationInput * _shipData.AngularSpeed * Time.fixedDeltaTime;
            _rbController.MoveRotation(_rbController.rotation + rotationAngle);
            _statisticsView.UpdateAngleRotation(_rbController.rotation);
            return true;
        }
        private bool TryMoveShip(float thrustInput)
        {
            if (thrustInput <= 0)
                return false;

            Vector2 forwardForce = -transform.up * _shipData.Speed * thrustInput * Time.fixedDeltaTime;
            _rbController.linearVelocity = forwardForce;
            _statisticsView.UpdateCoordinates(_rbController.position);
            _statisticsView.UpdateSpaceShipVelocity(_rbController.linearVelocity);
            return true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseEnemy someEnemy = collision.GetComponent<BaseEnemy>();

            if (someEnemy && !gameObject.GetComponentInChildren<LaserBullet>())
            {
                Die();
            }
        }
        private void Die()
        {
            _statisticsController = OnGameOver?.Invoke();
            _statisticsController.UpdateDestroyedEnemies();
            Destroy(gameObject);
        }
    }
}