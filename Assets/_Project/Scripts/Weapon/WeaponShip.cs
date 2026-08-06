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

        [SerializeField] protected int CountShoots;
        [SerializeField] protected int MaxBulletsCount = 50;
        [SerializeField] protected AssignmentMode AssignmentMode;

        [SerializeField] private float _timeBulletRecovery = 2f;

        protected BaseBullet ConcreteBulletPrefab;
        protected IResourceLoader ResourceLoaderService;
        protected IRemoteConfigService RemoteConfigService;
        protected GameOverPresenter GameOverPresenter;
        protected ShipStatisticPresenter ShipStatisticsPresenter;
        protected virtual float TimeBulletRecovery
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(AssignmentMode))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_bullet_config");
                    RemoteConfigFireball _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigFireball>(jsonConfig);
                    return _remoteConfigFireball.TimeBulletRecovery;
                }
                return TimeBulletRecovery;
            }
        }

        private WaitForSeconds _waitSecondsRecover;

        public virtual void Initialize(GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticsPresenter, BaseBullet concreteBullet, IResourceLoader resourceLoader, IRemoteConfigService remoteConfigService)
        { 
            ConcreteBulletPrefab = concreteBullet;
            ResourceLoaderService = resourceLoader;
            RemoteConfigService = remoteConfigService;
            this.GameOverPresenter = gameOverPresenter;
            _waitSecondsRecover = new WaitForSeconds(TimeBulletRecovery);
            ShipStatisticsPresenter = shipStatisticsPresenter;
            UpdateWeapon();
            StartCoroutine(RecoverMissile());
        }

        protected abstract void UpdateWeapon();

        protected IEnumerator RecoverMissile()
        {
            while (CountShoots < MaxBulletsCount)
            {
                yield return _waitSecondsRecover;
                CountShoots++;
                UpdateWeapon();
            }
        }
    }
}