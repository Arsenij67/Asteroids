using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AsteroidEnemy : BaseEnemy
    {
        [SerializeField] private MeteoriteEnemy _meteoriteExample;
        [SerializeField] private int _countMeteorites = 3;
        public override void Move(Transform transformEnd)
        {
            _rb2dEnemy.linearVelocity = transform.up * Time.fixedDeltaTime * _speed;
        }
        public override void TakeDamage(float damage)
        {
            if (damage >= _health)
            {
                SplitIntoMeteorites();
            }

            base.TakeDamage(damage);
        }
        private void SplitIntoMeteorites()
        {
            Vector2 startDir = (Vector2)transform.up + new Vector2(1f, 1f);
            for (int i = 0; i < _countMeteorites; i++)
            {
                MeteoriteEnemy meteorite = Instantiate(_meteoriteExample, transform.position, Quaternion.identity);
                meteorite.SetDirection(startDir += (Vector2)transform.up);
            }
        }
    }
}
