using Asteroid.Enemies;
using Asteroid.SpaceShip;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Generation
{
    [RequireComponent(typeof(ObstaclesGenerationData))]
    public class ObstaclesGenerationController
    {
        private event Action<EnemyController, BaseEnemy> OnEnemySpawned;
        private event Action<SpaceShipController> OnShipSpawned;

        private ObstaclesGenerationData _genData;
        private WaitForSeconds _waitSecondsGenFreq;
        public void Init(Action<SpaceShipController> actonCallBack)
        {
            //_genData = GetComponent<ObstaclesGenerationData>();
            OnShipSpawned = actonCallBack;
            _waitSecondsGenFreq = new WaitForSeconds(_genData.GenFrequency);
            GenerateShip(_genData.PlayerShipToGenerateNow);
            WaitForNextGeneration();
 
        }
        public void Init(Action<EnemyController, BaseEnemy> actionCallBack)
        {
            OnEnemySpawned = actionCallBack;
        }
        private Task WaitForNextGeneration()
        {
            while (true)
            {
                Task.Delay(_genData.GenFrequency*1000);
                GenerateObstacle(_genData.
                    ObstacleToGenerateNow);
            }
        }
        private void GenerateObstacle(BaseEnemy enemy)
        {
            if (_genData.DirectionToFly)
            {
                //BaseEnemy enemyScene = Instantiate(enemy,
                //    _genData.PointObstacleToGenerate, Quaternion.identity);

                //if (enemyScene.TryGetComponent(out EnemyController enemyController))
                //{
                //    OnEnemySpawned?.Invoke(enemyController, enemyScene);
                //}
                //Vector2 direction = _genData.DirectionToFly.position - enemyScene.transform.position;
                //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                //enemyScene._rb2dEnemy.MoveRotation(angle);
            }
        }
        private void GenerateShip(SpaceShipController shipController)
        {
            //SpaceShipController playerShip = Instantiate(shipController,_genData.PointShipToGenerate, Quaternion.identity);
            //OnShipSpawned?.Invoke(playerShip);
        }
    }
}