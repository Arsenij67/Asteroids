using UnityEngine;

namespace Asteroid.SpaceShip
{
    [CreateAssetMenu(fileName = "SpaceShipData", menuName = "ScriptableObjects/SpaceShipData")]
    public class SpaceShipData:ScriptableObject
    {
        [Header("Движение")]
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

    }
}