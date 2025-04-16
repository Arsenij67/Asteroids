using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public IWeaponStrategy[] weaponStrategies;
    private IWeaponStrategy currentWeaponStrategy;
    private void Awake()
    {
        weaponStrategies = GetComponents<IWeaponStrategy>();
        currentWeaponStrategy = weaponStrategies[0];
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            SetWeapon(weaponStrategies[0]);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(weaponStrategies[1]);
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire(currentWeaponStrategy);
        }
    }


    private void SetWeapon(IWeaponStrategy weaponStrategy)
    {
        currentWeaponStrategy = weaponStrategy;
    }
    private void Fire(IWeaponStrategy weaponStrategy)
    {
      
      weaponStrategy.Fire();
        
    }
}
