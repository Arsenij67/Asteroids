using UnityEngine;
using Asteroid.Enemies;
using Asteroid.SpaceShip;
using Asteroid.Services.RemoteConfig;
using Asteroid.Database;

namespace Asteroid.Generation
{
    [CreateAssetMenu(fileName = "EntitiesGenerationData", menuName = "ScriptableObjects/EntitiesGenerationData")]
    public class EntitiesGenerationData : ScriptableObject
    {
        [SerializeField] private AssignmentMode _assignmentMode;
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private GameObject[] _playerShips;
        [SerializeField] private float _generationFrequency;

        private IRemoteConfigService _remoteConfigService;
        public float GenerationFrequency 
        {
            get
            {
                if (_assignmentMode.Equals(AssignmentMode.RemoteConfig))
                {
                    string shipJson = _remoteConfigService.GetValue<string>("obstacles_config");
                    RemoteConfigObstacle _remoteConfigObstacle = JsonUtility.FromJson<RemoteConfigObstacle>(shipJson);
                    return _remoteConfigObstacle.GenerationFrequency;
                }
                else
                {
                    return _generationFrequency;
                }
            } 
        }
        [field: SerializeField] public Vector2[] GenerationVertices { get; private set; }

        public Transform EndPointToFly { get; private set; }
        public BaseEnemy ObstacleToGenerateNow => _obstacles[Random.Range(0, _obstacles.Length)].GetComponent<BaseEnemy>();
        public SpaceShipController PlayerShipToGenerateNow {
            get
            {
                if (_assignmentMode.Equals(AssignmentMode.RemoteConfig))
                {
                    string shipJson = _remoteConfigService.GetValue<string>("ship_config");
                    RemoteConfigShip _remoteConfigShip = JsonUtility.FromJson<RemoteConfigShip>(shipJson);
                    return _playerShips[_remoteConfigShip.ShipVariant].GetComponent<SpaceShipController>();
                }

                else
                {
                    return _playerShips[0].GetComponent<SpaceShipController>();
                }

            }
        }
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

        public void Initialize(IRemoteConfigService remoteConfig)
        {
            _remoteConfigService = remoteConfig;
        }

        public void Initialize(Transform EndPoint, IRemoteConfigService remoteConfig)
        {
            EndPointToFly = EndPoint;
            _remoteConfigService = remoteConfig;
        }

    } 
}