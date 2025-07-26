using Asteroid.Enemies;
using Asteroid.SpaceShip;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Asteroid.Generation
{
    public class EntitiesGenerationController
    {
        public event Action<EnemyController, BaseEnemy> OnEnemySpawned;
        public event Action<SpaceShipController> OnShipSpawned;

        private EntitiesGenerationData _generationData;
        private IResourceLoaderService _resourceLoaderService;
        private IInstanceLoader _instanceLoader;
        private CancellationTokenSource _cancellationTokenSource;
        public void Initialize(EntitiesGenerationData entitiesGenData,IResourceLoaderService resourceLoader,IInstanceLoader instanceLoader)
        {

            _generationData = entitiesGenData;
            _resourceLoaderService = resourceLoader;
            _instanceLoader = instanceLoader;
            _cancellationTokenSource = _instanceLoader.CreateInstance<CancellationTokenSource>();
            GenerateShip(_generationData.PlayerShipToGenerateNow);
            WaitForNextGeneration(_cancellationTokenSource.Token);
            
        }

        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask WaitForNextGeneration(CancellationToken tokenStop)
        {
            while (!tokenStop.IsCancellationRequested) {
                await UniTask.Delay(TimeSpan.FromSeconds(_generationData.GenerationFrequency),cancellationToken:tokenStop);
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