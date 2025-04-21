using Asteroid.Weapon;
using UnityEngine;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShipData : MonoBehaviour
    {
        [Header("Движение")]
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Vector2 DownLeftBorder { get; private set; }
        [field: SerializeField] public Vector2 UpRightBorder { get; private set; }

    }
}