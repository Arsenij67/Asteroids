using UnityEngine;

namespace Asteroid.Weapon
{
    public class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damage;
        [SerializeField] protected readonly float _maxDamage = Mathf.Infinity;
        [SerializeField] protected readonly float _maxSpeed = Mathf.Infinity;
        public virtual float Damage => Mathf.Clamp(_damage, 0, _maxDamage);
        public float Speed => Mathf.Clamp(_speed, 0, _maxSpeed);
    }
}
