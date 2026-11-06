using Asteroid.Enemies;
using UnityEngine;

namespace Asteroid.Weapon
{
    public class LaserBullet : BaseBullet
    {
        private bool _laserCollideNow = false;

        public override float Damage => _maxDamage;
        public bool LaserCollideNow => _laserCollideNow;

        public override void PlaySoundShoot()
        {
            AudioLocator.PlayLaserShoot();
        }

        protected override void ReactOnCollisionStart(Enemy enemy)
        {
            _laserCollideNow = enemy != null;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _laserCollideNow = !collision.gameObject.GetComponent<Enemy>();
        }
    }
}
