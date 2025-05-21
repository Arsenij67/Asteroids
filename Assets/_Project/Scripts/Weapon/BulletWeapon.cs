using UnityEngine;
using System;

namespace Asteroid.Weapon
{
    public class BulletWeaponController : WeaponShip, IWeaponStrategy
    {
        private event Action<FireballBullet, Vector2> OnBulletSpawnedCallback;

        [field: SerializeField] public short UniqueNumber { get; private set; }

        public void SetSpawnCallback(Action<FireballBullet, Vector2> callBack)
        {
            OnBulletSpawnedCallback = callBack;
        }

        public void Fire()
        {
            if (_countShoots > 0)
            {
                var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<FireballBullet>();
                OnBulletSpawnedCallback?.Invoke(bullet, -transform.up);
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