using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform ? shipTransform;
    private BaseEnemy enemy;

    private void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
        shipTransform = FindAnyObjectByType<SpaceShipController>()?.transform;
    }

    private void FixedUpdate()
    {
        if (shipTransform)
        {
            enemy.Move(shipTransform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<SpaceShipController>();
        BaseBullet bullet = collision.GetComponent<BaseBullet>();
        if (collision.GetComponent<BaseEnemy>() != null)
        {
            return;
        }
       
        if (bullet != null)
        {
            
            enemy.TakeDamage(bullet.Damage);
        }
       
        if (ship)
        {
            enemy.Die();
        }
    }
}
