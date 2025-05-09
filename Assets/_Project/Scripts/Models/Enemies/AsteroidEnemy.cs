using System;
using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(EnemyController))]
    public class AsteroidEnemy : BaseEnemy
    {
        [SerializeField] private MeteoriteEnemy _meteoriteExample;
        [SerializeField] private int _countMeteorites = 3;
        public void Init(Action<EnemyController, BaseEnemy> action,Vector2 directionFly)
        {
            OnEnemySpawned = action;
            Vector2 direction = directionFly - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _rb2dEnemy.MoveRotation(angle);
        }
        public override void Move(Transform transformEnd)
        {
            _rb2dEnemy.linearVelocity = transform.up * Time.fixedDeltaTime * _speed;
        }
        public override void TakeDamage(float damage)
        {
            if (damage >= _health)
            {
                SplitIntoMeteorites();
            }
            base.TakeDamage(damage);
        }
        private void SplitIntoMeteorites()
        {
            Vector2 startDir = (Vector2)transform.up + new Vector2(1f, 1f);
            for (int i = 0; i < _countMeteorites; i++)
            {
                MeteoriteEnemy meteorite = Instantiate(_meteoriteExample, transform.position, Quaternion.identity);
                meteorite.SetDirection(startDir += (Vector2)transform.up);
                OnEnemySpawned?.Invoke(meteorite.GetComponent<EnemyController>(), meteorite);
            }
        }
    }
}
