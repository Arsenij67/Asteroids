using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.UI;
using Asteroid.Weapon;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Asteroid.Generation
{
    public class EntitiesGenerationFactory :IDisposable
    {
        public event Action OnGameStarted;

        private EntitiesGenerationData _generationData;
        private IResourceLoader _resourceLoader;
        private IInstanceCreator _instanceCreator;
        private CancellationTokenSource _cancellationTokenSource;
        private ShipStatisticPresenter _shipStatisticPresenter;
        private EnemyDeathCounter _enemyDeathCounter;
        private GameOverPresenter _gameOverPresenter;
        private WeaponPresenter? _weaponController;
        private LaserWeaponController? _weaponShipLaser;
        private BulletWeaponController? _weaponShipBullet;
        private IRemoteConfigService _remoteConfig;
        private SpaceShipPresenter? _spaceShipPresenter;
        private ShipStatisticsView _shipStatisticView;
        private IDeviceInput _deviceInput;
        private SpaceShipData _spaceShipData;
        private AdvertisementPresenter _advertisementPresenter;
        private AnalyticsEventHandler _analyticsEventHandler;
        private bool _isGamePaused = false;
        private SpaceShipPresenter? _shipPrefab;
        private WeaponView _weaponView;

        public void Initialize(
            WeaponView weaponViewPrefab,
            AnalyticsEventHandler analyticsEventHandler,
            AdvertisementPresenter advertisementPresenter,
            EntitiesGenerationData entitiesGenerationData,
            IResourceLoader resourceLoader,
            IInstanceCreator instanceCreator,
            EnemyDeathCounter enemyDeathCounter,
            GameOverPresenter statisticPresenter,
            ShipStatisticPresenter shipStatisticPresenter,
            SpaceShipData spaceShipData,
            ShipStatisticsView shipStatisticView,
            IRemoteConfigService remoteConfig,
            IDeviceInput deviceInput)
        {
            _generationData = entitiesGenerationData;
            _resourceLoader = resourceLoader;
            _instanceCreator = instanceCreator;
            _enemyDeathCounter = enemyDeathCounter;
            _gameOverPresenter = statisticPresenter;
            _shipStatisticPresenter = shipStatisticPresenter;
            _remoteConfig = remoteConfig;
            _deviceInput = deviceInput; 
            _spaceShipData = spaceShipData;
            _shipStatisticView = shipStatisticView;
            _advertisementPresenter = advertisementPresenter;   
            _analyticsEventHandler = analyticsEventHandler;
            _cancellationTokenSource = _instanceCreator.CreateInstance<CancellationTokenSource>();
            _weaponView = weaponViewPrefab;
        }

        public void StartEnemiesCreation()
        {
            _isGamePaused = false;
            WaitAndGenerateNext(_cancellationTokenSource.Token);
          
        }

        public void Dispose()
        {   
            StopEnemiesCreation();
            _spaceShipPresenter.OnShipDied -= _analyticsEventHandler.SendEventGameEnd;
            OnGameStarted -= _analyticsEventHandler.SendEventGameStart;
            _spaceShipPresenter.OnShipDied -= PauseEnemiesCreation;
            _advertisementPresenter.OnPlayerRevived -= ReviveShip;
            _spaceShipPresenter.OnShipDied -= OnShipDestroyedHandler;
            _resourceLoader.UnloadAllResources();   
        }

        public SpaceShipPresenter CreateShip(SpaceShipPresenter shipControllerPrefab)
        {
            _shipPrefab = shipControllerPrefab;
            SpaceShipPresenter playerShip = _resourceLoader.
                Instantiate(shipControllerPrefab,
                _generationData.PointShipToGenerate,
                Quaternion.identity);
            _spaceShipPresenter = playerShip;
            return _spaceShipPresenter;
        }

        public void SubscribeShip()
        {
            _spaceShipPresenter.OnShipDied += PanelRestartSpawnedHandler;
            _spaceShipPresenter.OnShipDied += _analyticsEventHandler.SendEventGameEnd;
            _spaceShipPresenter.OnShipDied += PauseEnemiesCreation;
            _spaceShipPresenter.OnShipDied += UnsubscribeShip;
            _spaceShipPresenter.OnShipDied += OnShipDestroyedHandler;
            _spaceShipPresenter.OnShipSpawned += StartEnemiesCreation;
            _advertisementPresenter.OnPlayerRevived += ReviveShip;
            OnGameStarted += _analyticsEventHandler.SendEventGameStart;
            _spaceShipPresenter.OnShipSpawned += ShipInitializeHandler;
            _spaceShipPresenter.Initialize(_shipStatisticView, _deviceInput, _spaceShipData);
            OnGameStarted.Invoke();
        }

        private void PauseEnemiesCreation()
        {
            _isGamePaused = true;
        }

        private void StopEnemiesCreation()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask WaitAndGenerateNext(CancellationToken tokenStop)
        {
            while (!tokenStop.IsCancellationRequested! && !_isGamePaused)
            {

                await UniTask.Delay(TimeSpan.FromSeconds(_generationData.GenerationFrequency), cancellationToken: tokenStop);
                var enemy = CreateEnemy(_generationData.ObstacleToGenerateNow, _generationData.PointObstacleToGenerate, Quaternion.identity);
                SubscribeEnemy(enemy.GetComponent<EnemyController>(), enemy);
            }
        }

        private void ReviveShip()
        {
            CreateShip(_generationData.PlayerShipToGenerateNow);
            SubscribeShip();
        }

        private Enemy CreateEnemy(Enemy enemyPrefab, Vector3 position, Quaternion rotation)
        {
            Enemy enemyInstance = _resourceLoader.Instantiate(enemyPrefab, position, rotation);

            if (!enemyInstance.TryGetComponent(out EnemyController enemyController))
            {
                Debug.LogError($"Enemy {enemyInstance.name} has no EnemyController!");
                return enemyInstance;
            }

            return enemyInstance;
        }

        private void OnEnemyDestroyedHandler(Enemy destroyedEnemy)
        {
            _enemyDeathCounter.OnEnemyDied(destroyedEnemy);
            destroyedEnemy.OnEnemyDestroyed -= OnEnemyDestroyedHandler;
            _spaceShipPresenter.OnShipDied -= destroyedEnemy.Disappear;
        }

        private void UnsubscribeShip()
        {
            _spaceShipPresenter.OnShipDied -= PanelRestartSpawnedHandler;
            _spaceShipPresenter.OnShipDied -= _analyticsEventHandler.SendEventGameEnd;
            _spaceShipPresenter.OnShipDied -= PauseEnemiesCreation;
            _spaceShipPresenter.OnShipSpawned -= StartEnemiesCreation;
            OnGameStarted -= _analyticsEventHandler.SendEventGameStart;
            _spaceShipPresenter.OnShipSpawned -= ShipInitializeHandler;
            _spaceShipPresenter.OnShipDied -= UnsubscribeShip;
        }

        private void SubscribeEnemy(EnemyController enemyController, Enemy currentEnemy)
        {
            Transform shipTransform = _generationData.EndPointToFly;
            currentEnemy.Initialize(shipTransform, _gameOverPresenter, _shipStatisticPresenter);
            enemyController.Initialize(shipTransform);
            currentEnemy.Initialize(_generationData.EndPointToFly, _gameOverPresenter, _shipStatisticPresenter);
            enemyController.Initialize(_generationData.EndPointToFly);
            currentEnemy.OnEnemyDestroyed += OnEnemyDestroyedHandler;
            _spaceShipPresenter.OnShipDied += currentEnemy.Disappear;
        }

        private void OnShipDestroyedHandler()
        {
            _resourceLoader.UnloadResource(_shipPrefab.name);
        }

        private void ShipInitializeHandler()
        {
            _weaponController = _spaceShipPresenter.GetComponent<WeaponPresenter>();
            _weaponShipLaser = _spaceShipPresenter.GetComponent<LaserWeaponController>();
            _weaponShipBullet = _spaceShipPresenter.GetComponent<BulletWeaponController>();

            _generationData.Initialize(_remoteConfig, _spaceShipPresenter.transform);
            _weaponShipBullet.Initialize(_gameOverPresenter, _shipStatisticPresenter, _generationData.FireballPrefab, _resourceLoader, _remoteConfig);
            _weaponShipLaser.Initialize(_gameOverPresenter,_shipStatisticPresenter, _generationData.LaserPrefab, _resourceLoader, _remoteConfig);
            _weaponController.Initialize(_weaponView);
        }

        private async void PanelRestartSpawnedHandler()
        {
            _gameOverPresenter.OpenPanelRestart();
            await _gameOverPresenter.AddAndRefreshDestroyedEnemies();
        }
    }
}