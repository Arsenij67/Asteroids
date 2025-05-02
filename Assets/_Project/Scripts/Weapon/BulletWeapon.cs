using UnityEngine;

namespace Asteroid.Weapon
{
    public class BulletWeaponController : WeaponShip, IWeaponStrategy
    {
        private event System.Action<FireballBullet, Vector2> OnBulletSpawnedCallback;
        [field: SerializeField] public short UniqueNumber { get; private set; }
        public void SetSpawnCallback(System.Action<FireballBullet, Vector2> callback)
        {
            OnBulletSpawnedCallback = callback;
        }
        public void Fire()
        {
            if (_countShoots > 0)
            {
                var bullet = Instantiate(_bulletPref, transform.position, Quaternion.identity)
                    .GetComponent<FireballBullet>();
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