using Asteroid.Enemies;
using Asteroid.SpaceShip;
using System;
using System.Collections;
using UnityEngine;

namespace Asteroid.Generation
{
    public class EntitiesGenerationController : MonoBehaviour
    {
        public event Action<EnemyController, BaseEnemy> OnEnemySpawned;
        public event Action<SpaceShipController> OnShipSpawned;

        private EntitiesGenerationData _generationData;
        private WaitForSeconds _waitSecondsGenFreq;
        private IResourceLoaderService _resourceLoaderService;
        public void Initialize(EntitiesGenerationData entitiesGenData,IResourceLoaderService resourceLoader)
        {
            _generationData = entitiesGenData;
            _resourceLoaderService = resourceLoader;
            _waitSecondsGenFreq = new WaitForSeconds(_generationData.GenerationFrequency);
            GenerateShip(_generationData.PlayerShipToGenerateNow);
            StartCoroutine(WaitForNextGeneration());

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
            if (_generationData.EndPointToFly!=null)
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
            SpaceShipController playerShip = _resourceLoaderService.Instantiate(shipController, _generationData.PointShipToGenerate, Quaternion.identity).GetComponent<SpaceShipController>();
            OnShipSpawned?.Invoke(playerShip);
        }
    }
}