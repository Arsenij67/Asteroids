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
    public abstract class WeaponShip : MonoBehaviour
    {
        public Action<BaseBullet, Vector2> OnMissalSpawned;

        [SerializeField] protected int _countShoots;
        [SerializeField] protected int _maxBulletsCount = 50;
        [SerializeField] protected float _timeBulletRecovery = 2f;
        [SerializeField] protected AssignmentMode AssignmentMode;

        protected ShipStatisticsView _shipView;
        protected BaseBullet _concreteBulletPrefab;
        protected ShipStatisticsController _controllerStatistics;
        protected IResourceLoaderService _resourceLoaderService;
        protected IRemoteConfigService _remoteConfigService;

        private WaitForSeconds _waitSecondsRecover;

        protected virtual float TimeBulletRecovery
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(AssignmentMode))
                {
                    string jsonConfig = _remoteConfigService.GetValue<string>("weapon_bullet_config");
                    RemoteConfigFireball _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigFireball>(jsonConfig);
                    return _remoteConfigFireball.TimeBulletRecovery;
                }
                return _timeBulletRecovery;
            }
        }
 
        public virtual void Initialize(BaseBullet concreteBullet, ShipStatisticsView shipStView,ShipStatisticsController controllerStatistics, IResourceLoaderService resourceLoader, IRemoteConfigService remoteConfigService)
        { 
            _concreteBulletPrefab = concreteBullet;
            _shipView = shipStView;
            _resourceLoaderService = resourceLoader;
            _controllerStatistics = controllerStatistics;
            _remoteConfigService = remoteConfigService;
            _waitSecondsRecover = new WaitForSeconds(TimeBulletRecovery);
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