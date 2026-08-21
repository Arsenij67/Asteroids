using Asteroid.Generation;
using Asteroid.UI;
using System;
using UnityEngine;
 
namespace Asteroid.Weapon
{
    [RequireComponent(typeof(IWeaponStrategy))]
    public class WeaponPresenter : MonoBehaviour
    {
        private WeaponView _weaponView;
        private IWeaponStrategy [] _weaponStrategies;
        private IWeaponStrategy _currentWeaponStrategy;

        public void Initialize(WeaponView weaponView)
        {
            _weaponView = weaponView;
            _weaponStrategies = GetComponents<IWeaponStrategy>();
            _currentWeaponStrategy = _weaponStrategies[0];
            _weaponView.OnButtonFireballClicked+=FireFireball;
            _weaponView.OnButtonLaserClicked+=FireLaser;
            _weaponView.Initialize();
        }

        private void FireFireball()
        {
            SetWeapon(_weaponStrategies[0]);
            Fire(_currentWeaponStrategy);
        }

        private void FireLaser()
        {
            SetWeapon(_weaponStrategies[1]);
            Fire(_currentWeaponStrategy);
        }


        private void SetWeapon(IWeaponStrategy weaponStrategy)
        {
            _currentWeaponStrategy = weaponStrategy;
        }

        private void Fire(IWeaponStrategy weaponStrategy)
        {
            weaponStrategy.Fire();
        }

        private void OnDestroy()
        {
            _weaponView.OnButtonFireballClicked-=FireFireball;
            _weaponView.OnButtonFireballClicked-=FireLaser;
        }
    }
}