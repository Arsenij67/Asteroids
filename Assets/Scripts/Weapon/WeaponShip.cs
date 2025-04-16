using System.Collections;
using UnityEngine;

public abstract class WeaponShip : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPref;
    [SerializeField] protected int countShoots;
    [SerializeField] protected ShipStatisticsView shipView;
    [SerializeField] protected int maxBulletsCount = 50;
    [SerializeField] private float timeBulletRecovery = 2f;
    protected abstract void UpdateViewWeapon();

    protected IEnumerator RecoverBullet()
    {
        while (countShoots < maxBulletsCount)
        {
            yield return new WaitForSeconds(timeBulletRecovery);
            countShoots++;
            UpdateViewWeapon();
        }
    }

    private void Awake()
    {
        UpdateViewWeapon();
    }

}
