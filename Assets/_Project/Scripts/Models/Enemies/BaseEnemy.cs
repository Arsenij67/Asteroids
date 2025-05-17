using Asteroid.Statistic;
using UnityEngine;
using Asteroid.SpaceObjectActions;
using System;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEnemy : SpaceObject
    {
        protected Action<EnemyController, BaseEnemy> OnEnemySpawned;

        [SerializeField] protected float _health;
        [SerializeField] protected int _speed;

        protected Rigidbody2D _rigidBody2DEnemy;
        protected readonly int _maxSpeed = 1000;
        protected readonly int _maxHealth = 1000;

        private ShipStatisticsModel _shipStatisticModel;
        public int Speed => Mathf.Clamp(_speed, 0, _maxSpeed);
        public void Initialize(ShipStatisticsModel shipStModel)
        {
            _rigidBody2DEnemy = GetComponent<Rigidbody2D>();
            _shipStatisticModel = shipStModel;
        }
        public abstract void Move(Transform transformEnd = null);
        public virtual void TakeDamage(float damage)
        {
            damage = Mathf.Max(0, damage);
            if (_health > damage)
            {
                _health -= damage;
            }
            else
            {
                _health = 0;
                Die();
            }
        }
        public void Die()
        {
            _shipStatisticModel.EnemiesDestroyed++;
            Destroy(gameObject);
        }
    }
}