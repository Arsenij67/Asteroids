using Asteroid.Audio;
using Asteroid.Effects;
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
        public const short INDEX_CHILD_DISPLAY_ENEMY = 0;

        public event Action<Enemy> OnEnemyDestroyed;


        public float Damage;
        public bool EnemyIsDied = false;

        [SerializeField] private float _health;
        [SerializeField] private int _speed;

        protected GameOverPresenter GameOverPresenter;
        protected Rigidbody2D RigidBody2DEnemy;
        protected Transform TransformEnd;
        protected ShipStatisticPresenter ShipStatisticPresenter;
        protected IAudioService AudioLocator;

        protected float Speed => Mathf.Clamp(_speed, 0, Mathf.Infinity);
        protected float Health => Mathf.Clamp(_health, 0, Mathf.Infinity);

        private DisplayEnemy _displayEnemy;

        public virtual void Initialize(Transform transformEnd, GameOverPresenter gameOverPresenter, ShipStatisticPresenter shipStatisticPresenter, IAudioService audioLocator)
        {
            RigidBody2DEnemy = GetComponent<Rigidbody2D>();
            TransformEnd = transformEnd;
            GameOverPresenter = gameOverPresenter;
            ShipStatisticPresenter = shipStatisticPresenter;
            _displayEnemy = transform.GetChild(INDEX_CHILD_DISPLAY_ENEMY).GetComponent<DisplayEnemy>();
            _displayEnemy.Initialize();
            AudioLocator = audioLocator;
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
            PlayExploadSound();
            Destroy(gameObject, lifeTime);
        }

        public void Disappear()
        {
            Destroy(gameObject);
        }

        public virtual void PlayExploadSound()
        {
            AudioLocator.PlayExplosion();
        }    
    }
}