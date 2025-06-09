using Asteroid.Statistic;
using System;
using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(EnemyController))]
    public class AsteroidEnemy : BaseEnemy
    {
        public Action<BaseEnemy> OnMeteoriteDestroyed;

        [SerializeField] private MeteoriteEnemy _meteoriteExample;
        [SerializeField] private int _countMeteorites = 3;

        public void Initialize(ShipStatisticsModel shipStModel, Transform transformEnd, Action<BaseEnemy> destroyEnemyCallBack,Vector2 PointEndFly)
        {
            base.Initialize(shipStModel, transformEnd);
            Vector2 direction = PointEndFly - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _rigidBody2DEnemy.MoveRotation(angle);
        }

        public override void Move(Transform transformEnd)
        {
            _rigidBody2DEnemy.linearVelocity = transform.up * Time.fixedDeltaTime * Speed;
        }

        public override void TakeDamage(float damage)
        {
            if (damage >= Health)
            {
                SplitIntoMeteorites();
            }
            base.TakeDamage(damage);
        }

        private void SplitIntoMeteorites()
        {
            Vector2 offset = (Vector2)transform.up + (Vector2)transform.right;
            Vector2 startDir = (Vector2)transform.up + offset;
            for (int i = 0; i < _countMeteorites; i++)
            {
                MeteoriteEnemy meteorite = Instantiate(_meteoriteExample, transform.position, Quaternion.identity);
                EnemyController enemyController = meteorite.GetComponent<EnemyController>();

                meteorite.Initialize(_shipStatisticModel,_transformEnd);
                enemyController.Initialize(_transformEnd);

                meteorite.OnEnemyDestroyed += MeteoriteDestroyedHandler;
                meteorite.SetDirection(startDir += (Vector2)transform.up);
                 
            }
        }

        private void MeteoriteDestroyedHandler(BaseEnemy meteorite)
        {
            OnMeteoriteDestroyed?.Invoke(meteorite);
        }
    }
}
