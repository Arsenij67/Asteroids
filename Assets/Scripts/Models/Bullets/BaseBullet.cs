using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;

    [SerializeField] protected readonly int maxDamage = 999_999;
    [SerializeField] protected readonly int maxSpeed = 10_000;

    public virtual float Damage => Mathf.Clamp(damage,0,maxDamage);
    public float Speed => Mathf.Clamp(speed, 0, maxSpeed);


}
