
using UnityEngine;

namespace Asteroid.Weapon
{
    public class LaserBullet : BaseBullet
    {
        private const float OFFSET_COEFFICIENT = -2f;
        public Vector2 SpawnOffset => transform.right * OFFSET_COEFFICIENT * transform.localScale.x;
        public override float Damage => _maxDamage;
    }
}
