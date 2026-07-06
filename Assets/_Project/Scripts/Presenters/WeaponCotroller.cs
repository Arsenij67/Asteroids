using System;
using UnityEngine;
 
namespace Asteroid.Weapon
{
    public class WeaponController:MonoBehaviour
    {
        private IWeaponStrategy [] _weaponStrategies;
        private IWeaponStrategy _currentWeaponStrategy;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeapon(_weaponStrategies[0]);
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeapon(_weaponStrategies[1]);
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire(_currentWeaponStrategy);
            }
        }

        public void Initialize()
        {
            _weaponStrategies = GetComponents<IWeaponStrategy>();
            _currentWeaponStrategy = _weaponStrategies[0];
        }

        private void SetWeapon(IWeaponStrategy weaponStrategy)
        {
            _currentWeaponStrategy = weaponStrategy;
        }

        private void Fire(IWeaponStrategy weaponStrategy)
        {
            weaponStrategy.Fire();
        }
    }
}