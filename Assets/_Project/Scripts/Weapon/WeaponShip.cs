using System.Collections;
using UnityEngine;
using Asteroid.Statistic;

namespace Asteroid.Weapon
{
    public abstract class WeaponShip : MonoBehaviour
    {
        [SerializeField] protected GameObject _bulletPref;
        [SerializeField] protected int _countShoots;
        [SerializeField] protected ShipStatisticsView _shipView;
        [SerializeField] protected int _maxBulletsCount = 50;
        [SerializeField] private float _timeBulletRecovery = 2f;

        private WaitForSeconds _waitSecondsRecover;

        private void Awake()
        {
            _waitSecondsRecover = new WaitForSeconds(_timeBulletRecovery);
        }
        private void Start()
        {
            UpdateViewWeapon();
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