using Asteroid.SpaceShip;
using Asteroid.Weapon;
using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(BaseEnemy))]
    public class EnemyController : MonoBehaviour
    {
        private Transform? _shipTransform;
        private BaseEnemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<BaseEnemy>();
            _shipTransform = FindAnyObjectByType<SpaceShipController>()?.transform;
        }

        private void FixedUpdate()
        {
            if (_shipTransform)
            {
                _enemy.Move(_shipTransform);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ship = collision.gameObject.GetComponent<SpaceShipController>();
            BaseBullet bullet = collision.GetComponent<BaseBullet>();
            if (collision.TryGetComponent(out BaseEnemy enemy))
            {
                return;
            }

            if (bullet != null)
            {
                _enemy.TakeDamage(bullet.Damage);
            }

            if (ship)
            {
                _enemy.Die();
            }
        }
    }
}
