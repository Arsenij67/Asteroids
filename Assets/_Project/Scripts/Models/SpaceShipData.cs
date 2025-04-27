using UnityEngine;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShipData : MonoBehaviour
    {
        [Header("��������")]
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

    }
}