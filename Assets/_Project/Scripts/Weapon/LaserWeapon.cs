using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services.RemoteConfig;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using System;
using System.Collections;
using UnityEngine;

namespace Asteroid.Weapon
{
    public class LaserWeaponController : WeaponShip, IWeaponStrategy
    {
        public event Action OnLaserTurned;

        [SerializeField] private float _attackTime;

        private LaserBullet _laserObject;
        private bool _laserTurned;
        private WaitForSeconds _waitSecondsGlow;

        [field: SerializeField] public short UniqueNumber { get; private set; }

        public bool LaserTurned => _laserTurned;
        protected override float TimeBulletRecovery
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(AssignmentMode))
                {
                    string jsonConfig = _remoteConfigService.GetValue<string>("weapon_laser_config");
                    Debug.Log(jsonConfig);
                    RemoteConfigLaser _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigLaser>(jsonConfig);
                    return _remoteConfigFireball.TimeBulletRecovery;
                }
                return _timeBulletRecovery;
            }
        }
        private float AttackTime
        {
            get 
            {
                if (AssignmentMode.Equals(AssignmentMode.RemoteConfig))
                {
                    string jsonConfig = _remoteConfigService.GetValue<string>("weapon_laser_config");
                    RemoteConfigLaser _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigLaser>(jsonConfig);
                    return _remoteConfigFireball.AttackTime;
                }
                return _attackTime;
            }
        }

        public override void Initialize(BaseBullet concreteBullet, ShipStatisticsView shipStView, ShipStatisticsController controllerStatistics, IResourceLoaderService resourceLoader, IRemoteConfigService remoteConfigService)
        {
            base.Initialize(concreteBullet, shipStView,controllerStatistics, resourceLoader,remoteConfigService);
            _waitSecondsGlow = new WaitForSeconds(AttackTime);
            _laserObject = _resourceLoaderService.Instantiate(_concreteBulletPrefab, transform).GetComponent<LaserBullet>();
            _laserObject.gameObject.SetActive(false);
            _laserObject.transform.position = (Vector2)transform.position + _laserObject.SpawnOffset;
            _laserObject.Initialize(remoteConfigService);
        }

         public void Fire()
        {
            if (!_laserTurned)
            {
                OnLaserTurned?.Invoke();
                OnMissalSpawned?.Invoke(_concreteBulletPrefab, transform.up * -1);
                _laserTurned = true;
                StartCoroutine(FireLaser());
                _controllerStatistics.IncreaseCountLaserShoots();
            }
        }

        private IEnumerator FireLaser()
        {
            if (_countShoots > 0)
            {
                _laserObject.gameObject.SetActive(true);
                yield return _waitSecondsGlow;
                _laserObject.gameObject.SetActive(false);
                _laserTurned = false;
                _countShoots--;
                UpdateViewWeapon();
            }
        }

        protected override void UpdateViewWeapon()
        {
            _shipView.UpdateLaserCount(_countShoots);
            _shipView.UpdateRollbackTime(_attackTime);
        }
    }
}