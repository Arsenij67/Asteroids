using UnityEngine;
using Asteroid.Enemies;

namespace Asteroid.Generation
{
    public class ObstaclesGenerationData : MonoBehaviour
    {
        [SerializeField] private GameObject[] _obstacles;
        [field: SerializeField] public float GenFrequency { get; private set; }
        [field: SerializeField] public Transform[] GenerationVertices { get; private set; }
        [field: SerializeField] public Transform DirectionToFly { get; private set; }
        public BaseEnemy ObstacleToGenerateNow => _obstacles[Random.Range(0, _obstacles.Length)].GetComponent<BaseEnemy>();
        public Vector2 PointToGenerate
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
    }
}