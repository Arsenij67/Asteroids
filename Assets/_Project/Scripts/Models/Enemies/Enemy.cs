using Asteroid.Effects;
using Asteroid.Generation;
using Asteroid.SpaceObjectActions;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using System;
using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : SpaceObject
    {
        private const int CHILD_INDEX_DISPLAY = 0;

        public event Action<Enemy> OnEnemyDestroyed;


        [field: SerializeField] public float Damage;

        [SerializeField] private float _health;
        [SerializeField] private int _speed;

        protected GameOverPresenter GameOverPresenter;
        protected Rigidbody2D RigidBody2DEnemy;
        protected Transform TransformEnd;
        protected ShipStatisticPresenter ShipStatisticPresenter;

        public bool EnemyIsDied { get; private set; }

        protected float Speed => Mathf.Clamp(_speed, 0, Mathf.Infinity);
        protected float Health => Mathf.Clamp(_health, 0, Mathf.Infinity);

        private DisplayEnemy _displayEnemy;
        private IInstanceCreator _instanceCreator;
 

        public virtual void Initialize(IInstanceCreator instanceCreator,IResourceLoader resourceLoader, Transform transformEnd, GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticPresenter)
        {
            RigidBody2DEnemy = GetComponent<Rigidbody2D>();
            _instanceCreator = instanceCreator;
            TransformEnd = transformEnd;
            GameOverPresenter = gameOverPresenter;
            ShipStatisticPresenter = shipStatisticPresenter;
            _displayEnemy = _instanceCreator.CreateInstance<DisplayEnemy>();
            _displayEnemy.Initialize(resourceLoader,transform.GetChild(CHILD_INDEX_DISPLAY).GetComponent<Animator>(),this);
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

        public void Die(float lifeTime = 0.7f)
        {
            EnemyIsDied = true;
            OnEnemyDestroyed?.Invoke(this);
            _displayEnemy.PlayDieEffect();
            Destroy(gameObject,lifeTime);
        }

        public void Clear()
        {
            Destroy(gameObject);
        }
    }
}