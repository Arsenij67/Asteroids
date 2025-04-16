using UnityEngine;

public class BulletWeaponController : WeaponShip, IWeaponStrategy
{
    private short uniqueNumber = 0;
    public short UniqueNumber => uniqueNumber;

    private void Start()
    {
        StartCoroutine(RecoverBullet());
    }

    public void Fire()
    {
        if (countShoots > 0)
        {
            Rigidbody2D rb = Instantiate(bulletPref, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rb.linearVelocity = rb.GetComponent<FireballBullet>().Speed * -transform.up * Time.fixedDeltaTime;
            Destroy(rb.gameObject, 5f);
            countShoots--;
            UpdateViewWeapon();
        }

    }

    protected override void UpdateViewWeapon()
    {
        shipView.UpdateFireballCount(countShoots);
    }

}
