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

        [SerializeField] private float _timeBulletRecovery = 2f;

        private WaitForSeconds _waitSecondsRecover;

        public virtual void Initialize(BaseBullet concreteBullet, ShipStatisticsView shipStView)
        { 
            _concreteBulletPrefab = concreteBullet;
            _waitSecondsRecover = new WaitForSeconds(_timeBulletRecovery);
            _shipView = shipStView;
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