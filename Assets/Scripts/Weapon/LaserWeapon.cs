using System.Collections;
using UnityEngine;

public class LaserWeaponController: WeaponShip, IWeaponStrategy
{
    private GameObject laserObject;
    private bool laserTurned = false;
    private short uniqueNumber = 1;

    [SerializeField] private float glowDuration;
    public short UniqueNumber => uniqueNumber;

    private  void Start()
    {
        laserObject = Instantiate(bulletPref, transform);
        laserObject.SetActive(false);
        laserObject.transform.position = transform.position;
        StartCoroutine(RecoverBullet());
    }
    public void Fire()
    {
        if (!laserTurned)
        {
            StartCoroutine(FireLaser());
            laserTurned = true;
        }
    }
    private IEnumerator FireLaser()
    {
        if (countShoots > 0)
        {
            laserObject.SetActive(true);
            yield return new WaitForSeconds(glowDuration);
            laserObject.SetActive(false);
            laserTurned = false;
            countShoots--;
            UpdateViewWeapon();
        }

    }


    protected override void UpdateViewWeapon()
    {
        shipView.UpdateLaserCount(countShoots);
        shipView.UpdateRollbackTime(glowDuration);

    }
}
