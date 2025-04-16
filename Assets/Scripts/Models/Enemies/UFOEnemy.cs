using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class UFOEnemy : BaseEnemy
{
    public override void Move(Transform transformEnd)
    {

        Vector2 transformStart = transform.position;
        Vector2 direction = (Vector2)transformEnd.position - transformStart;
        Vector2 forwardForce = direction.normalized * Speed * Time.fixedDeltaTime;
        if (direction.sqrMagnitude > 0.05)
        {
            rb2dEnemy.linearVelocity = forwardForce;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        }

  

    }





}
