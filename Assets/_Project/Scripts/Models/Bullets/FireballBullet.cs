using Asteroid.Enemies;
using UnityEngine;


namespace Asteroid.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class FireballBullet : BaseBullet
    {
        private readonly float _lifeTime = 10f;
        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out BaseEnemy baseEnemy))
            {
                Destroy(gameObject);
            }
        }
    }
}
