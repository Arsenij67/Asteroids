using Asteroid.Statistic;
using UnityEngine;
using Asteroid.SpaceObjectActions;
using System;
using Unity.Mathematics;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEnemy : SpaceObject
    {
        public event Action<BaseEnemy> OnEnemyDestroyed;

        [SerializeField] private float _health;
        [SerializeField] private int _speed;

        protected Rigidbody2D _rigidBody2DEnemy;

        protected ShipStatisticsModel _shipStatisticModel;
        protected Transform _transformEnd;

        protected float Speed => Mathf.Clamp(_speed, 0, Mathf.Infinity);
        protected float Health => Mathf.Clamp(_health, 0, Mathf.Infinity);

        public void Initialize(ShipStatisticsModel shipStModel, Transform transformEnd)
        {
            _rigidBody2DEnemy = GetComponent<Rigidbody2D>();
            _shipStatisticModel = shipStModel;
            _transformEnd = transformEnd;
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
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}