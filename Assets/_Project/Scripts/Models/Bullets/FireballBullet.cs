using Asteroid.Enemies;
using UnityEngine;

namespace Asteroid.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FireballBullet : BaseBullet
    {
        private Rigidbody2D _rigidBody2D;
        private float _lifeTime = 5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out BaseEnemy enemy))
            {
                Destroy(gameObject);
            }
        }

        public override void Initialize(Vector2 direction, float speed, float damage)
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _speed = speed;
            _damage = damage;
            _rigidBody2D.linearVelocity = direction.normalized * _speed;

            Destroy(gameObject, _lifeTime);
        }
        
    }
}