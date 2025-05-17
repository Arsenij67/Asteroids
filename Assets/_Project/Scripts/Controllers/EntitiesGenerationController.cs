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

        private EntitiesGenerationData _generationData;
        private WaitForSeconds _waitSecondsGenFreq;
        public void Initialize(Action<SpaceShipController> actonCallBack, EntitiesGenerationData eGenData)
        {
            _generationData = eGenData;
            OnShipSpawned = actonCallBack;
            _waitSecondsGenFreq = new WaitForSeconds(_generationData.GenerationFrequency);
            GenerateShip(_generationData.PlayerShipToGenerateNow);
            StartCoroutine(WaitForNextGeneration());

        }
        public void Initialize(Action<EnemyController, BaseEnemy> actionCallBack)
        {
            OnEnemySpawned = actionCallBack;
        }
        private IEnumerator WaitForNextGeneration()
        {
            while (true)
            {
                yield return _waitSecondsGenFreq;
                GenerateObstacle(_generationData.
                    ObstacleToGenerateNow);
            }
        }
        private void GenerateObstacle(BaseEnemy enemy)
        {
            if (_generationData.DirectionToFly)
            {
                BaseEnemy enemyScene = Instantiate(enemy,
                    _generationData.PointObstacleToGenerate, Quaternion.identity);

                if (enemyScene.TryGetComponent(out EnemyController enemyController))
                {
                    OnEnemySpawned?.Invoke(enemyController, enemyScene);
                }
            }
        }
        private void GenerateShip(SpaceShipController shipController)
        {
            SpaceShipController playerShip = Instantiate(shipController, _generationData.PointShipToGenerate, Quaternion.identity);
            OnShipSpawned?.Invoke(playerShip);
        }
    }
}