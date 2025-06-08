using UnityEngine;
using System;

namespace Asteroid.Weapon
{
    public class BulletWeaponController : WeaponShip, IWeaponStrategy
    {
        [field: SerializeField] public short UniqueNumber { get; private set; }

        public void Fire()
        {
            if (_countShoots > 0)
            {
                var bullet = _resourceLoaderService.Instantiate(_concreteBulletPrefab, transform.position, Quaternion.identity).GetComponent<FireballBullet>();
                OnMissalSpawned?.Invoke(bullet, -transform.up);
                _countShoots--;
                UpdateViewWeapon();
            }
        }

        protected override void UpdateViewWeapon()
        {
            _shipView.UpdateFireballCount(_countShoots);
        }
    }
}