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
    public class SpaceShipController : SpaceObject
    {
        public event Action OnPlayerDie;

        private IDeviceInput _deviceInput;
        private IResourceLoaderService _loaderService;
        private ShipStatisticsView _statisticsView;
        private ShipStatisticsController _statisticsController;
        private SpaceShipData _shipData;
        private Rigidbody2D _rigidBody2D;
        private WeaponShip _laserWeaponController;


        private void FixedUpdate()
        {
            TryTeleport(transform.position);
            TryRotate(_deviceInput.ScanRotation());
            TryMove(_deviceInput.ScanMove());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseEnemy someEnemy = collision.GetComponent<BaseEnemy>();
            LaserWeaponController laserController = _laserWeaponController as LaserWeaponController;
            if (someEnemy!=null && !laserController.LaserTurned)
            {
                Die();
            }
        }

        public void Initialize(ShipStatisticsView statisticView, IDeviceInput concreteInput, ShipStatisticsController statisticController, WeaponShip laserWeaponController, IResourceLoaderService loader)
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _deviceInput = concreteInput;
            _statisticsView = statisticView;
            _statisticsController = statisticController;
            _laserWeaponController = laserWeaponController;
            _loaderService = loader;
            _shipData = _loaderService.LoadResource<SpaceShipData>("ScriptableObjects/SpaceShipData");
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

        private void Die()
        {
            OnPlayerDie?.Invoke();
            _statisticsController.UpdateDestroyedEnemies();
            Destroy(gameObject);
            
        }
    }
}