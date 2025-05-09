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
        private void FixedUpdate()
        {
            if (_shipTransform)
            {
                _enemy.Move(_shipTransform);
                _enemy.TryTeleport(_enemy.transform.position);
            }
        }
        public void Init(Transform shipTransform)
        {
            _enemy = GetComponent<BaseEnemy>();
            _shipTransform = shipTransform;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out BaseEnemy enemy))
            {
                return;
            }

            if (collision.TryGetComponent(out BaseBullet bullet))
            {
                _enemy.TakeDamage(bullet.Damage);
            }

            if (collision.TryGetComponent(out SpaceShipController ship))
            {
                _enemy.Die();
            }
        }
    }
}
