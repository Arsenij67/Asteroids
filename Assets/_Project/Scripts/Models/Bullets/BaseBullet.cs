using Asteroid.Database;
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

        public virtual float Damage => Mathf.Clamp(_damage, 0, _maxDamage);

        public virtual void Initialize(IRemoteConfigService remoteConfigService)
        {
            _remoteConfigService = remoteConfigService;
        }
    }
}
