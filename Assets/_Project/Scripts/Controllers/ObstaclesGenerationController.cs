using Asteroid.Enemies;
using System.Collections;
using UnityEngine;

namespace Asteroid.Generation
{

    [RequireComponent(typeof(ObstaclesGenerationData))]
    public class ObstaclesGenerationController : MonoBehaviour
    {
        private ObstaclesGenerationData _genData;
        private WaitForSeconds _waitSecondsGenFreq;
        private void Awake()
        {
            _genData = GetComponent<ObstaclesGenerationData>();
            _waitSecondsGenFreq = new WaitForSeconds(_genData.GenFrequency);
        }

        private void Start()
        {
            StartCoroutine(WaitForNextGeneration());
        }

        private IEnumerator WaitForNextGeneration()
        {
            while (true)
            {
                yield return _waitSecondsGenFreq;
                GenerateObstacle(_genData.ObstacleToGenerateNow);
            }

        }
        private void GenerateObstacle(BaseEnemy enemy)
        {
            if (_genData.DirectionToFly)
            {
                BaseEnemy enemyScene = Instantiate(enemy,
                    _genData.PointToGenerate, Quaternion.identity);

                Vector2 direction = _genData.DirectionToFly.position - enemyScene.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                enemyScene._rb2dEnemy.MoveRotation(angle);
            }
        }


    }
}