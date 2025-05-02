using UnityEngine;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShipData
    {
        [Header("Движение")]
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

    }
}