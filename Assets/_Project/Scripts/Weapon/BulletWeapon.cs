using UnityEngine;

namespace Asteroid.Weapon
{
    public class BulletWeaponController : WeaponShip, IWeaponStrategy
    {
        [field: SerializeField] public short UniqueNumber { get; private set;}
        private void Start()
        {
            StartCoroutine(RecoverBullet());
        }
        public void Fire()
        {
            if (_countShoots > 0)
            {
                Rigidbody2D rb = Instantiate(_bulletPref, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                rb.linearVelocity = rb.GetComponent<FireballBullet>().Speed * -transform.up * Time.fixedDeltaTime;
                Destroy(rb.gameObject, 5f);
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