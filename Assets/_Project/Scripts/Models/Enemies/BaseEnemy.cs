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
        protected event Action<BaseEnemy> OnEnemyDestroyed;

        [SerializeField] protected float _health;
        [SerializeField] protected int _speed;

        protected Rigidbody2D _rigidBody2DEnemy;
        protected readonly float _maxSpeed =  Mathf.Infinity;
        protected readonly float _maxHealth = Mathf.Infinity;

        protected ShipStatisticsModel _shipStatisticModel;
        protected Transform _transformEnd;

        public float Speed => Mathf.Clamp(_speed, 0, _maxSpeed);

        public void Initialize(ShipStatisticsModel shipStModel, Transform transformEnd, Action<BaseEnemy> destroyEnemyCallBack)
        {
            _rigidBody2DEnemy = GetComponent<Rigidbody2D>();
            _shipStatisticModel = shipStModel;
            _transformEnd = transformEnd;
            OnEnemyDestroyed = destroyEnemyCallBack;
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