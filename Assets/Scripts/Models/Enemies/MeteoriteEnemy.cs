using UnityEngine;

public class MeteoriteEnemy : BaseEnemy
{
    [SerializeField] private float rotationSpeed = 2f;
    private Vector2 dir = Vector2.zero;
    private readonly float lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject,lifeTime);
    }
    public override void Move(Transform transformEnd = null)
    {
       rb2dEnemy.linearVelocity = dir.normalized * Time.fixedDeltaTime * speed;
       Rotate(rotationSpeed);
    }
    public void SetDirection(Vector2 dir)
    {
        this.dir = dir;
    }
    public void Rotate(float angleOffset)
    {
        rb2dEnemy.MoveRotation(rb2dEnemy.rotation + (angleOffset * Time.fixedDeltaTime));
    }

    
}
