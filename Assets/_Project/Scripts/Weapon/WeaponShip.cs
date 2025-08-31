using Asteroid.Generation;
using Asteroid.Services.RemoteConfig;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using System;
using System.Collections;
using UnityEngine;

namespace Asteroid.Weapon
{
    public abstract class WeaponShip : MonoBehaviour
    {
        public Action<BaseBullet, Vector2> OnMissalSpawned;

        [SerializeField] protected int _countShoots;
        [SerializeField] protected int _maxBulletsCount = 50;

        protected ShipStatisticsView _shipView;
        protected BaseBullet _concreteBulletPrefab;
        protected ShipStatisticsController _controllerStatistics;
        protected IResourceLoaderService _resourceLoaderService;
        protected IRemoteConfigService _remoteConfigService;

        [SerializeField] private float _timeBulletRecovery = 2f;

        private WaitForSeconds _waitSecondsRecover;

        public virtual void Initialize(BaseBullet concreteBullet, ShipStatisticsView shipStView,ShipStatisticsController controllerStatistics, IResourceLoaderService resourceLoader, IRemoteConfigService remoteConfigService)
        { 
            _concreteBulletPrefab = concreteBullet;
            _waitSecondsRecover = new WaitForSeconds(_timeBulletRecovery);
            _shipView = shipStView;
            _resourceLoaderService = resourceLoader;
            _controllerStatistics = controllerStatistics;
            _remoteConfigService = remoteConfigService;
            UpdateViewWeapon();
            StartCoroutine(RecoverMissile());
        }

        protected abstract void UpdateViewWeapon();

        protected IEnumerator RecoverMissile()
        {
            while (_countShoots < _maxBulletsCount)
            {
                yield return _waitSecondsRecover;
                _countShoots++;
                UpdateViewWeapon();
            }
        }
    }
}