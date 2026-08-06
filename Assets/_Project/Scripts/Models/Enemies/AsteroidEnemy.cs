using Asteroid.SpaceShip;
using Asteroid.Statistic;
using System;
using UnityEngine;
using Zenject;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(EnemyController))]
    public class AsteroidEnemy : Enemy
    {
        public Action<Enemy> OnMeteoriteDestroyed;

        [SerializeField] private MeteoriteEnemy _meteoriteExample;
        [SerializeField] private int _countMeteorites = 3;

        private Vector2 _direction;

        public override void Initialize(Transform transformEnd, GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticPresenter)
        {
            base.Initialize(transformEnd, gameOverPresenter, shipStatisticPresenter);    
            _direction = (TransformEnd.position - transform.position).normalized;
        }

        public override void Move(Transform transformEnd)
        {
            RigidBody2DEnemy.linearVelocity = _direction * Time.fixedDeltaTime * Speed;
        }

        public override void TakeDamage(float damage)
        {
            if (damage >= Health)
            {
                SplitIntoMeteorites();
            }
            base.TakeDamage(damage);
        }

        public override void AddToStatistic()
        {
            ShipStatisticPresenter.IncreaseCountAsteroidsDestroyed();
        }

        private void SplitIntoMeteorites()
        {
            Vector2 offset = (Vector2)transform.up + (Vector2)transform.right;
            Vector2 startDir = (Vector2)transform.up + offset;
            for (int i = 0; i < _countMeteorites; i++)
            {
                MeteoriteEnemy meteorite = Instantiate(_meteoriteExample, transform.position, Quaternion.identity);
                EnemyController enemyController = meteorite.GetComponent<EnemyController>();

                meteorite.Initialize(TransformEnd,GameOverPresenter, ShipStatisticPresenter);
                enemyController.Initialize(TransformEnd);

                meteorite.OnEnemyDestroyed += MeteoriteDestroyedHandler;
                meteorite.SetDirection(startDir += (Vector2)transform.up);
                 
            }
        }

        private void MeteoriteDestroyedHandler(Enemy meteorite)
        {
            OnMeteoriteDestroyed?.Invoke(meteorite);
        }
    }
}
