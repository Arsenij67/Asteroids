using UnityEngine;

namespace Asteroid.Weapon
{
    public interface IWeaponStrategy
    {
        void Fire();
        short UniqueNumber { get; }

    }
}
