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
        [SerializeField] protected int _damage;
        [SerializeField] protected int _speed;

        protected Rigidbody2D _rb2dEnemy;
        protected readonly int _maxDamage = 999_999;
        protected readonly int _maxSpeed = 1000;
        protected readonly int _maxHealth = 1000;

        private ShipStatisticsModel _shipStModel; 
        public float Health => Mathf.Clamp(_health, 0, _maxHealth);
        public virtual int Damage => _maxDamage;
        public int Speed => Mathf.Clamp(_speed, 0, _maxSpeed);
        public void Init(ShipStatisticsModel shipStModel)
        {
            _rb2dEnemy = GetComponent<Rigidbody2D>();
            _shipStModel = shipStModel;
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
            _shipStModel._enemiesDestroyed++;
            Destroy(gameObject);
        }
    }
}