using Asteroid.Enemies;
using Asteroid.SpaceShip;
using System;
using System.Collections;
using UnityEngine;

namespace Asteroid.Generation
{
    public class EntitiesGenerationController : MonoBehaviour
    {
        private event Action<EnemyController, BaseEnemy> OnEnemySpawned;
        private event Action<SpaceShipController> OnShipSpawned;

        private EntitiesGenerationData _genData;
        private WaitForSeconds _waitSecondsGenFreq;
        public void Init(Action<SpaceShipController> actonCallBack, EntitiesGenerationData eGenData)
        {
            _genData = eGenData;
            OnShipSpawned = actonCallBack;
            _waitSecondsGenFreq = new WaitForSeconds(_genData.GenFrequency);
            GenerateShip(_genData.PlayerShipToGenerateNow);
            StartCoroutine(WaitForNextGeneration());

        }
        public void Init(Action<EnemyController, BaseEnemy> actionCallBack)
        {
            OnEnemySpawned = actionCallBack;
        }
        private IEnumerator WaitForNextGeneration()
        {
            while (true)
            {
                yield return _waitSecondsGenFreq;
                GenerateObstacle(_genData.
                    ObstacleToGenerateNow);
            }
        }
        private void GenerateObstacle(BaseEnemy enemy)
        {
            if (_genData.DirectionToFly)
            {
                BaseEnemy enemyScene = Instantiate(enemy,
                    _genData.PointObstacleToGenerate, Quaternion.identity);

                if (enemyScene.TryGetComponent(out EnemyController enemyController))
                {
                    OnEnemySpawned?.Invoke(enemyController, enemyScene);
                }
            }
        }
        private void GenerateShip(SpaceShipController shipController)
        {
            SpaceShipController playerShip = Instantiate(shipController, _genData.PointShipToGenerate, Quaternion.identity);
            OnShipSpawned?.Invoke(playerShip);
        }
    }
}