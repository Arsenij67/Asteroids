using Asteroid.Audio;
using Asteroid.Database;
using Asteroid.Enemies;
using Asteroid.Services.RemoteConfig;
using UnityEngine;

namespace Asteroid.Weapon
{
    public class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected AssignmentMode _assignmentMode;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damage;
        [SerializeField] protected readonly float _maxDamage = Mathf.Infinity;
        [SerializeField] protected readonly float _maxSpeed = Mathf.Infinity;

        protected IRemoteConfigService _remoteConfigService;

        protected virtual float Speed => Mathf.Clamp(_speed, 0, _maxSpeed);

        private IAudioService _audioLocator;

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

        public virtual void Initialize(IRemoteConfigService remoteConfigService, IAudioService audioLocator)
        {
            _remoteConfigService = remoteConfigService;
            _audioLocator = audioLocator;
        }

        public void PlaySoundShoot()
        {
            _audioLocator.PlayShoot();
        }
    }
}
