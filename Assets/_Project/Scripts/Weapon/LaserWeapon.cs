using Asteroid.Audio;
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

        public bool LaserCollideNow => _laserObject.LaserCollideNow;

        protected override float TimeBulletRecovery
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(AssignmentMode))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_laser_config");
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

        public override void Initialize(GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticsPresenter, BaseBullet concreteBullet, IResourceLoader resourceLoader, IRemoteConfigService remoteConfigService, IAudioService audioService)
        {
            base.Initialize(gameOverPresenter, shipStatisticsPresenter, concreteBullet, resourceLoader, remoteConfigService,audioService);
            _waitSecondsGlow = new WaitForSeconds(AttackTime);
            _laserObject = ResourceLoaderService.Instantiate(ConcreteBulletPrefab, transform).GetComponent<LaserBullet>();
            _laserObject.gameObject.SetActive(false);
            _laserObject.Initialize(remoteConfigService,audioService);
        }

         public void Fire()
        {
            if (!_laserTurned)
            {
                OnLaserTurned?.Invoke();
                OnMissalSpawned?.Invoke(ConcreteBulletPrefab, transform.up * -1);
                _laserTurned = true;
                StartCoroutine(FireLaser());
                PlayFireSound(_laserObject);
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

        protected override void  PlayFireSound(BaseBullet bullet)
        {
            bullet.PlaySoundShoot();
        }
    }
}