using UnityEngine;
using System;

namespace Asteroid.Weapon
{
    public class BulletWeaponController : WeaponShip, IWeaponStrategy
    {
        public void Fire()
        {
            if (CountShoots > 0)
            {
                var bullet = ResourceLoaderService.Instantiate(ConcreteBulletPrefab, transform.position, Quaternion.identity).GetComponent<FireballBullet>();
                OnMissalSpawned?.Invoke(bullet, -transform.up);
                CountShoots--;
                bullet.Initialize(-transform.up,RemoteConfigService,AudioLocator);
                PlayFireSound(bullet);
                UpdateWeapon();
            }
        }

        protected override void PlayFireSound(BaseBullet bullet)
        {
            bullet.PlaySoundShoot();
        }

        protected override void UpdateWeapon()
        {
            ShipStatisticsPresenter.UpdateCountBulletShoots(CountShoots);
        }
    }
}