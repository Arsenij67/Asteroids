using Asteroid.Audio;
using Asteroid.Database;
using Asteroid.Enemies;
using Asteroid.Services.RemoteConfig;
using UnityEngine;

namespace Asteroid.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FireballBullet : BaseBullet
    {
        private Rigidbody2D _rigidBody2D;
        private float _lifeTime = 5f;

        public override float Damage
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(_assignmentMode))
                {    
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_bullet_config");
                    RemoteConfigFireball _remoteConfigFireball =  JsonUtility.FromJson<RemoteConfigFireball>(jsonConfig);
                    return _remoteConfigFireball.Damage;
                }

                else
                {
                    return Mathf.Clamp(_damage, 0, _maxDamage);
                }
            }

        }

        protected override float Speed
        {
            get 
            {
                if (AssignmentMode.RemoteConfig.Equals(_assignmentMode))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_bullet_config");
                    RemoteConfigFireball _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigFireball>(jsonConfig);
                    return _remoteConfigFireball.Speed;
                }

                else
                {
                    return Mathf.Clamp(_speed, 0, _maxSpeed);
                }
            }
        }

        private float LifeTime
        {
            get
            {
                if (AssignmentMode.RemoteConfig.Equals(_assignmentMode))
                {
                    string jsonConfig = RemoteConfigService.GetValue<string>("weapon_bullet_config");
                    RemoteConfigFireball _remoteConfigFireball = JsonUtility.FromJson<RemoteConfigFireball>(jsonConfig);
                    return _remoteConfigFireball.LifeTime;
                }

                else
                {
                    return Mathf.Max(0,_lifeTime);
                }
            }
        }

        public void Initialize(Vector2 direction, IRemoteConfigService remoteConfigService,IAudioService audioLocator)
        {
            base.Initialize(remoteConfigService,audioLocator);
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _rigidBody2D.linearVelocity = direction.normalized * Speed;
            Destroy(gameObject, LifeTime);
        }

        public override void PlaySoundShoot()
        {
            AudioLocator.PlayFireballShoot();
        }
    }
}