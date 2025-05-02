using UnityEngine;
using Asteroid.Enemies;
using Asteroid.SpaceShip;

namespace Asteroid.Generation
{
    public class ObstaclesGenerationData
    {
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private GameObject[] _playerShips;
        [field: SerializeField] public int GenFrequency { get; private set; }
        [field: SerializeField] public Transform[] GenerationVertices { get; private set; }
        public Transform DirectionToFly { get; private set; }
        public BaseEnemy ObstacleToGenerateNow => _obstacles[Random.Range(0, _obstacles.Length)].GetComponent<BaseEnemy>();
        public SpaceShipController PlayerShipToGenerateNow => _playerShips[0].GetComponent<SpaceShipController>();
        public Vector2 PointObstacleToGenerate
        {
            get
            {
                int startIndex = Random.Range(0, GenerationVertices.Length);
                int endIndex = (startIndex + 1) % GenerationVertices.Length;

                Vector3 startPoint = GenerationVertices[startIndex].position;
                Vector3 endPoint = GenerationVertices[endIndex].position;

                float t = Random.Range(0f, 1f);
                return Vector2.Lerp(startPoint, endPoint, t);
            }
        }
        public Vector2 PointShipToGenerate
        {
            get
            {
                return new Vector2(
                    (GenerationVertices[0].transform.position.x + GenerationVertices[2].transform.position.x) / 2,
                    (GenerationVertices[0].transform.position.y + GenerationVertices[2].transform.position.y) / 2);
            }
        }
        public void Init(Transform directionToFly)
        {
            DirectionToFly = directionToFly;
        }
    } 
}