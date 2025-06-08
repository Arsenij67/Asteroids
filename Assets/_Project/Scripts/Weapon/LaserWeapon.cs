using Asteroid.Statistic;
using System.Collections;
using UnityEngine;

namespace Asteroid.Weapon
{
    public class LaserWeaponController : WeaponShip, IWeaponStrategy
    {
        [SerializeField] private float _glowDuration;

        private LaserBullet _laserObject;
        private bool _laserTurned;
        private WaitForSeconds _waitSecondsGlow;

        [field: SerializeField] public short UniqueNumber { get; private set; }

        public bool LaserTurned => _laserTurned;

        public override void Initialize(BaseBullet concreteBullet, ShipStatisticsView shipStView)
        {
            base.Initialize(concreteBullet, shipStView);

            _waitSecondsGlow = new WaitForSeconds(_glowDuration);
            _laserObject = Instantiate(_concreteBulletPrefab, transform).GetComponent<LaserBullet>();
            _laserObject.gameObject.SetActive(false);
            _laserObject.transform.position = (Vector2)transform.position + _laserObject.SpawnOffset;
        }

         public void Fire()
        {
            if (!_laserTurned)
            {
                OnMissalSpawned?.Invoke(_concreteBulletPrefab, transform.up * -1);
                _laserTurned = true;
                StartCoroutine(FireLaser());
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
            _shipView.UpdateRollbackTime(_glowDuration);
        }
    }
}