using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb2dEnemy;
    [SerializeField] protected float health;
    [SerializeField] protected int damage;
    [SerializeField] protected int speed;
    
    private ShipStatisticsModel shipStModel;

    protected readonly int maxDamage = 999_999;
    protected readonly int maxSpeed = 1000;
    public float Health => Mathf.Clamp(health, 0, 100);
    public virtual int Damage => Mathf.Clamp(damage, 0, maxDamage);
    public int Speed => Mathf.Clamp(speed, 0, maxSpeed);
    public abstract void Move(Transform transformEnd=null);
    public virtual void TakeDamage(float damage)
    {
        damage = Mathf.Max(0, damage);
        if (health > damage)
        {
            health -= damage;
        }

        else
        {
            health = 0;
            Die();
        }

    }
    public void Die()
    {
        shipStModel.enemiesDestroyed++;
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb2dEnemy = GetComponent<Rigidbody2D>();
        shipStModel = FindFirstObjectByType<ShipStatisticsModel>(FindObjectsInactive.Include);
    }

}
