
namespace Asteroid.Weapon
{
    
    public interface IWeaponStrategy
    {
        public void Fire();
        
        public short UniqueNumber { get; }

    }
}
