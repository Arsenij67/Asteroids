using UnityEngine;

namespace Asteroid.Weapon
{
    public class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damage;

        [SerializeField] protected readonly int _maxDamage = 999_999;
        [SerializeField] protected readonly int _maxSpeed = 10_000;
        public virtual float Damage => Mathf.Clamp(_damage, 0, _maxDamage);
        public float Speed => Mathf.Clamp(_speed, 0, _maxSpeed);


    }
}
