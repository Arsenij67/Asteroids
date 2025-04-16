using UnityEngine;
 
[RequireComponent(typeof(CircleCollider2D))]
public class AsteroidEnemy : BaseEnemy
{
   
    [SerializeField] private MeteoriteEnemy[] meteorites;
    public override int Damage => maxDamage;

    private readonly float lifeTime = 20f;
    public override void Move(Transform transformEnd)
    {
        rb2dEnemy.linearVelocity = transform.up * Time.fixedDeltaTime * speed;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    public override void TakeDamage(float damage)
    {
        
        if (damage >= health)
        {
            
            SplitIntoMeteorites();    
        }

        base.TakeDamage(damage);
    }

    private void SplitIntoMeteorites()
    {
        Vector2 startDir = (Vector2)transform.up + new Vector2(0.5f,0.5f);

        foreach (var meteorite in meteorites) 
        {
            if (meteorite == null) continue;

            foreach (Component comp in meteorite?.GetComponents<Component>())
            {
                // MonoBehaviour (скрипты)
                if (comp is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.enabled = true;
                }
                // Collider (2D/3D)
                else if (comp is Collider collider)
                {
                    collider.enabled = true;
                }
                else if (comp is Collider2D collider2D)
                {
                    collider2D.enabled = true;
                }
                // Renderer (MeshRenderer, SpriteRenderer и т. д.)
                else if (comp is Renderer renderer)
                {
                    renderer.enabled = true;
                }
                // Behaviour ( включает AudioSource, Animator и др.)
                else if (comp is Behaviour behaviour)
                {
                    behaviour.enabled = true;
                }
            }
            meteorite.transform.SetParent(null);
        
            meteorite.SetDirection(startDir+=(Vector2)transform.up);
           
        }
        
        }
}
