using UnityEngine;
using Asteroid.Enemies;
using Asteroid.SpaceShip;

namespace Asteroid.Generation
{
    [CreateAssetMenu(fileName = "EntitiesGenerationData", menuName = "ScriptableObjects/EntitiesGenerationData")]
    public class EntitiesGenerationData : ScriptableObject
    {
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private GameObject[] _playerShips;
        [field: SerializeField] public int GenerationFrequency { get; private set; }
        [field: SerializeField] public Vector2[] GenerationVertices { get; private set; }
        public Transform DirectionToFly { get; private set; }
        public BaseEnemy ObstacleToGenerateNow => _obstacles[Random.Range(0, _obstacles.Length)].GetComponent<BaseEnemy>();
        public SpaceShipController PlayerShipToGenerateNow => _playerShips[0].GetComponent<SpaceShipController>();
        public Vector2 PointObstacleToGenerate
        {
            get
            {
                int startIndex = Random.Range(0, GenerationVertices.Length);
                int endIndex = (startIndex + 1) % GenerationVertices.Length;

                Vector3 startPoint = GenerationVertices[startIndex];
                Vector3 endPoint = GenerationVertices[endIndex];

                float t = Random.Range(0f, 1f);
                return Vector2.Lerp(startPoint, endPoint, t);
            }
        }
        public Vector2 PointShipToGenerate
        {
            get
            {
                return new Vector2(
                    (GenerationVertices[0].x + GenerationVertices[2].x) / 2,
                    (GenerationVertices[0].y + GenerationVertices[2].y) / 2);
            }
        }
        public void Initialize(Transform directionToFly)
        {
            DirectionToFly = directionToFly;
        }
    } 
}