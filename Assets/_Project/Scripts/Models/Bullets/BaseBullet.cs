using Asteroid.Audio;
using Asteroid.Database;
using Asteroid.Enemies;
using Asteroid.Services.RemoteConfig;
using UnityEngine;

namespace Asteroid.Weapon
{
    public abstract class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected AssignmentMode _assignmentMode;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damage;
        [SerializeField] protected readonly float _maxDamage = Mathf.Infinity;
        [SerializeField] protected readonly float _maxSpeed = Mathf.Infinity;

        protected IRemoteConfigService RemoteConfigService;
        protected IAudioService AudioLocator;

        protected virtual float Speed => Mathf.Clamp(_speed, 0, _maxSpeed);

 

        public virtual float Damage => Mathf.Clamp(_damage, 0, _maxDamage);

        private void OnTriggerEnter2D(Collider2D collision)
        {
             ReactOnCollisionStart(collision.GetComponent<Enemy>());
        }

        protected virtual void ReactOnCollisionStart(Enemy enemy)
        {
            if (enemy != null)
            {
                Destroy(gameObject);
            }
        }

        public abstract void PlaySoundShoot();

        public virtual void Initialize(IRemoteConfigService remoteConfigService, IAudioService audioLocator)
        {
            RemoteConfigService = remoteConfigService;
            AudioLocator = audioLocator;
        }

 
    }
}
