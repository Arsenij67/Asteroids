using Asteroid.Statistic;
using UnityEngine;
using Asteroid.SpaceObjectActions;
using System;
using Unity.Mathematics;
using Asteroid.SpaceShip;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : SpaceObject
    {

        public event Action<Enemy> OnEnemyDestroyed;

        [SerializeField] private float _health;
        [SerializeField] private int _speed;

        protected GameOverPresenter GameOverPresenter;
        protected Rigidbody2D RigidBody2DEnemy;
        protected Transform TransformEnd;
        protected ShipStatisticPresenter ShipStatisticPresenter;

        protected float Speed => Mathf.Clamp(_speed, 0, Mathf.Infinity);
        protected float Health => Mathf.Clamp(_health, 0, Mathf.Infinity);

        public virtual void Initialize(Transform transformEnd, GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticPresenter)
        {
            RigidBody2DEnemy = GetComponent<Rigidbody2D>();
            TransformEnd = transformEnd;
            GameOverPresenter = gameOverPresenter;
            ShipStatisticPresenter = shipStatisticPresenter;   
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
                Die();
            }
        }

        public void Die()
        {
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public void Disappear()
        {
            Destroy(gameObject);
        }

    }
}