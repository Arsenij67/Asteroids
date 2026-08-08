using UnityEngine;
using Asteroid.Statistic;
using Asteroid.Enemies;
using Asteroid.Weapon;
using Asteroid.Inputs;
using Asteroid.SpaceObjectActions;
using System;
using Asteroid.Generation;
using System.Collections.Generic;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(LaserWeaponController))]
    public class SpaceShipPresenter : SpaceObject
    {
        public event Action OnShipDied;
        public event Action OnShipSpawned;

        private IDeviceInput _deviceInput;
        private ShipStatisticsView _statisticsView;
        private SpaceShipData _shipData;
        private Rigidbody2D _rigidBody2D;
        private LaserWeaponController _weaponController;

        private void FixedUpdate()
        {
            TryTeleport(transform.position);
            TryRotate(_deviceInput.ScanRotation());
            TryMove(_deviceInput.ScanMove());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy someEnemy = collision.GetComponent<Enemy>();
            if (someEnemy != null && !_weaponController.LaserCollideNow)
            {
                    Die();
            }
        }

        public void Initialize(ShipStatisticsView statisticView, IDeviceInput concreteInput, SpaceShipData shipData)
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _weaponController = GetComponent<LaserWeaponController>();
            _deviceInput = concreteInput;
            _statisticsView = statisticView;
            _shipData = shipData;
            OnShipSpawned?.Invoke();
        }

        private void TryRotate(float intensityInput)
        {
            if (!Mathf.Approximately(intensityInput, 0f))
            { 
                float rotationAngle = -intensityInput * _shipData.AngularSpeed * Time.fixedDeltaTime;
                _rigidBody2D.MoveRotation(_rigidBody2D.rotation + rotationAngle);
                _statisticsView.UpdateAngleRotation(_rigidBody2D.rotation);
             }
        }

        private void TryMove(float intensityInput)
        {
            if (intensityInput > 0)
            {
                Vector2 forwardForce = -transform.up * _shipData.Speed * intensityInput * Time.fixedDeltaTime;
                _rigidBody2D.linearVelocity = forwardForce;
                _statisticsView.UpdateCoordinates(_rigidBody2D.position);
                _statisticsView.UpdateSpaceShipVelocity(_rigidBody2D.linearVelocity);
            }
        }

        public void Die()
        {
            OnShipDied?.Invoke();
            Destroy(gameObject);
        }
    }
}