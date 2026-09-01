using Asteroid.Effects;
using Asteroid.SpaceShip;
using Asteroid.Weapon;
using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(DisplayEnemy))]
    public class EnemyController : MonoBehaviour
    {
        private Transform? _shipTransform;
        private Enemy _enemy;
        private void FixedUpdate()
        {
            if (_shipTransform!=null)
            {
                _enemy.Move(_shipTransform);
                _enemy.TryTeleport(_enemy.transform.position);
            }
        }
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Enemy enemy))
            {
                return;
            }

            if (collision.TryGetComponent(out BaseBullet bullet) || collision.transform.parent!=null ? collision.transform.parent.TryGetComponent(out bullet): false)
            {
                _enemy.TakeDamage(bullet.Damage);
            }

            if (collision.TryGetComponent(out SpaceShipPresenter ship))
            {
                DieEnemy(_enemy);
            }
        }

        public void Initialize(Transform shipTransform)
        {
            _enemy = GetComponent<Enemy>();
            _shipTransform = shipTransform;
        }

        private void DieEnemy(Enemy enemy)
        {
            enemy.Die();
        }
    }
}
