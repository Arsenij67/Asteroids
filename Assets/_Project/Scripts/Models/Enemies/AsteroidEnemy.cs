using Asteroid.Audio;
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
        private const float SPREAD_RANGE = 70f;

        public Action<Enemy> OnMeteoriteDestroyed;

        [SerializeField] private MeteoriteEnemy _meteoriteExample;
        [SerializeField] private int _countMeteorites = 3;

        private Vector2 _direction;

        public override void Initialize(Transform transformEnd, GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticPresenter, IAudioService audioService)
        {
            base.Initialize(transformEnd, gameOverPresenter, shipStatisticPresenter,audioService);    
            _direction = (TransformEnd.position - transform.position).normalized;
        }

        public override void Move(Transform transformEnd)
        {
            if (!EnemyIsDied)
            {
                RigidBody2DEnemy.linearVelocity = _direction * Time.fixedDeltaTime * Speed;
            }
            else
            {
                RigidBody2DEnemy.linearVelocity = Vector2.zero;
            }
        }

        public override void TakeDamage(float damage)
        {
            if (damage >= Health)
            {
                SplitIntoMeteorites(SPREAD_RANGE);
            }
            base.TakeDamage(damage);
        }

        public override void AddToStatistic()
        {
            ShipStatisticPresenter.IncreaseCountAsteroidsDestroyed();
        }

        private void SplitIntoMeteorites(float angleRange)
        {
            if (_countMeteorites <= 0 || _meteoriteExample == null) return;

            float baseAngle = Mathf.Atan2(_direction.y, _direction.x);

            float angleStep = angleRange * Mathf.Deg2Rad / (_countMeteorites - 1);

            float startAngle = baseAngle - (angleRange * Mathf.Deg2Rad);

            for (int i = 0; i < _countMeteorites; ++i)
            {
                float currentAngle = startAngle + i * angleStep;

                Vector2 direction = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));

                MeteoriteEnemy meteorite = Instantiate(_meteoriteExample, transform.position, Quaternion.identity);
                EnemyController enemyController = meteorite.GetComponent<EnemyController>();

                meteorite.Initialize(TransformEnd, GameOverPresenter, ShipStatisticPresenter,AudioLocator);
                enemyController.Initialize(TransformEnd);

                meteorite.OnEnemyDestroyed += MeteoriteDestroyedHandler;

                meteorite.SetDirection(direction.normalized);
            }
        }

        private void MeteoriteDestroyedHandler(Enemy meteorite)
        {
            OnMeteoriteDestroyed?.Invoke(meteorite);
            meteorite.OnEnemyDestroyed -= MeteoriteDestroyedHandler;
        }
    }
}
