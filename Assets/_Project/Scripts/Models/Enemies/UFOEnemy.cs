using UnityEngine;

namespace Asteroid.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UFOEnemy : Enemy
    {
        private const float MIN_LENGHT_REACT = 0.05f;

        public override void AddToStatistic()
        {
            ShipStatisticPresenter.IncreaseCountUFODestroyed();
        }

        public override void Move(Transform transformEnd)
        {
            Vector2 transformStart = transform.position;
            Vector2 direction = (Vector2)transformEnd.position - transformStart;
            Vector2 forwardForce = direction.normalized * Speed * Time.fixedDeltaTime;
            if (direction.sqrMagnitude > MIN_LENGHT_REACT)
            {
                RigidBody2DEnemy.linearVelocity = forwardForce;
                Rotate(forwardForce);
            }
        }

        private void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            RigidBody2DEnemy.MoveRotation(angle);
        }
    }
}
