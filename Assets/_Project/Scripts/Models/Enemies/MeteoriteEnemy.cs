using UnityEngine;

namespace Asteroid.Enemies
{
    public class MeteoriteEnemy : BaseEnemy
    {
        [SerializeField] private float _rotationSpeed = 2f;

        private Vector2 _direction = Vector2.zero;
        public override void Move(Transform transformEnd = null)
        {
            _rigidBody2DEnemy.linearVelocity = _direction.normalized * Time.fixedDeltaTime * _speed;
            Rotate(_rotationSpeed);
        }
        public void SetDirection(Vector2 dir)
        {
            _direction = dir;
        }
        public void Rotate(float angleOffset)
        {
            _rigidBody2DEnemy.MoveRotation(_rigidBody2DEnemy.rotation + (angleOffset * Time.fixedDeltaTime));
        }
    }
}
