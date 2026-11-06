using UnityEngine;

namespace Asteroid.Enemies
{
    public class MeteoriteEnemy : Enemy
    {
        [SerializeField] private float _rotationSpeed = 2f;

        private Vector2 _direction = Vector2.zero;

        public override void Move(Transform transformEnd = null)
        {
            if (!EnemyIsDied)
            {
                RigidBody2DEnemy.linearVelocity = _direction.normalized * Time.fixedDeltaTime * Speed;
                Rotate(_rotationSpeed);
            }
            else
            {
                RigidBody2DEnemy.linearVelocity = Vector2.zero;
            }
        }

        public void SetDirection(Vector2 dir)
        {
            _direction = dir;
        }

        public void Rotate(float angleOffset)
        {
            RigidBody2DEnemy.MoveRotation(RigidBody2DEnemy.rotation + (angleOffset * Time.fixedDeltaTime));
        }

        public override void AddToStatistic()
        {
            ShipStatisticPresenter.IncreaseCountMeteoritesDestroyed();
        }
    }
}
