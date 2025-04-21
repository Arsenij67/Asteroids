using UnityEngine;

namespace Asteroid.Enemies
{
    public class MeteoriteEnemy : BaseEnemy
    {
        [SerializeField] private float _rotationSpeed = 2f;

        private Vector2 _dir = Vector2.zero;
        private readonly float _lifeTime = 10f;

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }
        public override void Move(Transform transformEnd = null)
        {
            _rb2dEnemy.linearVelocity = _dir.normalized * Time.fixedDeltaTime * _speed;
            Rotate(_rotationSpeed);
        }
        public void SetDirection(Vector2 dir)
        {
            _dir = dir;
        }
        public void Rotate(float angleOffset)
        {
            _rb2dEnemy.MoveRotation(_rb2dEnemy.rotation + (angleOffset * Time.fixedDeltaTime));
        }


    }
}
