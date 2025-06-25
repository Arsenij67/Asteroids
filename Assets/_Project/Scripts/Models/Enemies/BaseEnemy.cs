using Asteroid.Statistic;
using UnityEngine;
using Asteroid.SpaceObjectActions;
using System;
using Unity.Mathematics;
using Asteroid.SpaceShip;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEnemy : SpaceObject
    {

        public event Action<BaseEnemy> OnEnemyDestroyed;

        [SerializeField] private float _health;
        [SerializeField] private int _speed;

        protected ShipStatisticsController _shipStatisticController;
        protected Rigidbody2D _rigidBody2DEnemy;
        protected Transform _transformEnd;

        protected float Speed => Mathf.Clamp(_speed, 0, Mathf.Infinity);
        protected float Health => Mathf.Clamp(_health, 0, Mathf.Infinity);

        public void Initialize(Transform transformEnd, ShipStatisticsController shipStatisticModel)
        {
            _rigidBody2DEnemy = GetComponent<Rigidbody2D>();
            _transformEnd = transformEnd;
            _shipStatisticController = shipStatisticModel;
        }

        public abstract void Move(Transform transformEnd = null);
        public abstract void AddToStatistic();

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
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

    }
}