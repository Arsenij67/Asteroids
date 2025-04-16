using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(BoxCollider2D))]
public class SpaceShipData : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Vector2 downLeftBorder;
    [SerializeField] private Vector2 upRightBorder;

    [Header("������")]
    public IWeaponStrategy[] weaponStrategy;
    private int countWeapons;
    public float AngularSpeed=>angularSpeed;
    public float Speed => speed;
    public int CountWeapons => countWeapons;
    public Vector2 DownLeftBorder => downLeftBorder;
    public Vector2 UpRightBorder => upRightBorder;

    void Awake()
    {
        weaponStrategy = GetComponents<IWeaponStrategy>();
        countWeapons = weaponStrategy.Length;
    }

}
