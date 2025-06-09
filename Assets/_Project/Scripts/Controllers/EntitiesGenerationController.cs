using Asteroid.Enemies;
using Asteroid.SpaceShip;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    public class EntitiesGenerationController
    {
        public event Action<EnemyController, BaseEnemy> OnEnemySpawned;
        public event Action<SpaceShipController> OnShipSpawned;

        private EntitiesGenerationData _generationData;
        private IResourceLoaderService _resourceLoaderService;
        public void Initialize(EntitiesGenerationData entitiesGenData,IResourceLoaderService resourceLoader)
        {
            _generationData = entitiesGenData;
            _resourceLoaderService = resourceLoader;
            GenerateShip(_generationData.PlayerShipToGenerateNow);
            WaitForNextGeneration();
            
        }

        private async UniTask WaitForNextGeneration()
        {
            while (true) {
                await UniTask.Delay(TimeSpan.FromSeconds(_generationData.GenerationFrequency));
                GenerateObstacle(_generationData.ObstacleToGenerateNow);
            }
          
        }

        private void GenerateObstacle(BaseEnemy enemy)
        {
            if (_generationData.EndPointToFly!=null)
            {
                BaseEnemy enemyScene = _resourceLoaderService.Instantiate(enemy,
                    _generationData.PointObstacleToGenerate, Quaternion.identity).GetComponent<BaseEnemy>();

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