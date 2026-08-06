using Asteroid.Database;
using Asteroid.Enemies;
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

        public bool LaserTurned =>  _laserTurned;
        protected override float TimeBulletRecovery
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(AssignmentMode))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_laser_config");
                    Debug.Log(jsonConfig);
                    RemoteConfigLaser _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigLaser>(jsonConfig);
                    return _remoteConfigFireball.TimeBulletRecovery;
                }
                return TimeBulletRecovery;
            }
        }
        private float AttackTime
        {
            get 
            {
                if (AssignmentMode.Equals(AssignmentMode.RemoteConfig))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_laser_config");
                    RemoteConfigLaser _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigLaser>(jsonConfig);
                    return _remoteConfigFireball.AttackTime;
                }
                return _attackTime;
            }
        }

        public override void Initialize(GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticsPresenter, BaseBullet concreteBullet, IResourceLoader resourceLoader, IRemoteConfigService remoteConfigService)
        {
            base.Initialize(gameOverPresenter, shipStatisticsPresenter, concreteBullet, resourceLoader, remoteConfigService);
            _waitSecondsGlow = new WaitForSeconds(AttackTime);
            _laserObject = ResourceLoaderService.Instantiate(ConcreteBulletPrefab, transform).GetComponent<LaserBullet>();
            _laserObject.gameObject.SetActive(false);
            _laserObject.transform.position = (Vector2)transform.position + _laserObject.SpawnOffset;
            _laserObject.Initialize(remoteConfigService);
        }

         public void Fire()
        {
            if (!_laserTurned)
            {
                OnLaserTurned?.Invoke();
                OnMissalSpawned?.Invoke(ConcreteBulletPrefab, transform.up * -1);
                _laserTurned = true;
                StartCoroutine(FireLaser());
                
            }
        }

        private IEnumerator FireLaser()
        {
            if (CountShoots > 0)
            {
                _laserObject.gameObject.SetActive(true);
                yield return _waitSecondsGlow;
                _laserObject.gameObject.SetActive(false);
                CountShoots--;
                UpdateWeapon();
            }
            _laserTurned = false;
        }

        protected override void UpdateWeapon()
        {
            ShipStatisticsPresenter.UpdateCountLaserShoots(CountShoots);
            ShipStatisticsPresenter.UpdateRollbackTime(AttackTime);
        }
    }
}