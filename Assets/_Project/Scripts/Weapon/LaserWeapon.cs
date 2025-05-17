using System.Collections;
using UnityEngine;

namespace Asteroid.Weapon
{
    public class LaserWeaponController : WeaponShip, IWeaponStrategy
    {
        [SerializeField] private float _glowDuration;

        private GameObject _laserObject;
        private bool _laserTurned;
        private WaitForSeconds _waitSecondsGlow;
        [field: SerializeField] public short UniqueNumber { get; private set; }
        public void Initialize()
        {
            _waitSecondsGlow = new WaitForSeconds(_glowDuration);
            _laserObject = Instantiate(_bulletPrefab, transform);
            _laserObject.SetActive(false);
            _laserObject.transform.position = transform.position;
            StartCoroutine(RecoverBullet());
        }
        public void Fire()
        {
            if (!_laserTurned)
            {
                StartCoroutine(FireLaser());
                _laserTurned = true;
            }
        }
        private IEnumerator FireLaser()
        {
            if (_countShoots > 0)
            {
                _laserObject.SetActive(true);
                yield return _waitSecondsGlow;
                _laserObject.SetActive(false);
                _laserTurned = false;
                _countShoots--;
                UpdateViewWeapon();
            }
        }
        protected override void UpdateViewWeapon()
        {
            _shipView.UpdateLaserCount(_countShoots);
            _shipView.UpdateRollbackTime(_glowDuration);
        }
    }
}