using System.Collections;
using UnityEngine;
using Asteroid.Statistic;

namespace Asteroid.Weapon
{
    public abstract class WeaponShip : MonoBehaviour
    {
        [SerializeField] protected int _countShoots;
        [SerializeField] protected int _maxBulletsCount = 50;

        protected ShipStatisticsView _shipView;
        protected GameObject _bulletPrefab;

        [SerializeField] private float _timeBulletRecovery = 2f;

        private WaitForSeconds _waitSecondsRecover;
        public void Initialize(GameObject bulletPref, ShipStatisticsView shipStView)
        { 
            _bulletPrefab = bulletPref;
            _waitSecondsRecover = new WaitForSeconds(_timeBulletRecovery);
            _shipView = shipStView;
            UpdateViewWeapon();
            StartCoroutine(RecoverBullet());
        }
        protected abstract void UpdateViewWeapon();
        protected IEnumerator RecoverBullet()
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