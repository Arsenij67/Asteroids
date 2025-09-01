using UnityEngine;

namespace Asteroid.Database
{
    [System.Serializable]
    public struct RemoteConfigFireball
    {
        public float LifeTime;
        public float Speed;
        public float Damage;
        public float TimeBulletRecovery;
    }
}