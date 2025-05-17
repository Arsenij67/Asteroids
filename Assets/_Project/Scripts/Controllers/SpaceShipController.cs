using UnityEngine;
using Asteroid.Statistic;
using Asteroid.Enemies;
using Asteroid.Weapon;
using Asteroid.Inputs;
using Asteroid.SpaceObjectActions;
using System;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShipController : SpaceObject
    {
        private event Action OnGameOver;

        private IDeviceInput _deviceInput;
        private ShipStatisticsView _statisticsView;
        private ShipStatisticsController _statisticsController;
        private SpaceShipData _shipData;
        private Rigidbody2D _rigidBody2D;
        private void FixedUpdate()
        {
            TryTeleport(transform.position);
            RotateShipKeyBoard(_deviceInput.ScanRotation());
            MoveShipKeyBoard(_deviceInput.ScanMove());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseEnemy someEnemy = collision.GetComponent<BaseEnemy>();

            if (someEnemy && !gameObject.GetComponentInChildren<LaserBullet>())
            {
                Die();
            }
        }
        public void Initialize(Action callBack, ShipStatisticsView statisticView, IDeviceInput concreteInput, ShipStatisticsController statisticController)
        {
            OnGameOver = callBack;
            _shipData = Resources.Load<SpaceShipData>("ScriptableObjects/SpaceShipData");
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _deviceInput = concreteInput;
            _statisticsView = statisticView;
            _statisticsController = statisticController;
        }
        private void RotateShipKeyBoard(float intensityInput)
        {
            if (!Mathf.Approximately(intensityInput, 0f))
            { 
                float rotationAngle = -intensityInput * _shipData.AngularSpeed * Time.fixedDeltaTime;
                _rigidBody2D.MoveRotation(_rigidBody2D.rotation + rotationAngle);
                _statisticsView.UpdateAngleRotation(_rigidBody2D.rotation);
             }
        }
        private void MoveShipKeyBoard(float intensityInput)
        {
            if (intensityInput > 0)
            {
                Vector2 forwardForce = -transform.up * _shipData.Speed * intensityInput * Time.fixedDeltaTime;
                _rigidBody2D.linearVelocity = forwardForce;
                _statisticsView.UpdateCoordinates(_rigidBody2D.position);
                _statisticsView.UpdateSpaceShipVelocity(_rigidBody2D.linearVelocity);
            }
        }
        private void Die()
        {
            OnGameOver?.Invoke();
            _statisticsController.UpdateDestroyedEnemies();
            Destroy(gameObject);
        }
    }
}