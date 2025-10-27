using Asteroid.Database;
using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using System;
using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class SpaceEntryPoint : MonoBehaviour
    {
        public event Action OnGameStarted;
        public event Action OnPlayerDied;


        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;
        [SerializeField] private RectTransform _UIParent;
        [SerializeField] private ShipStatisticsView _shipStatisticViewPrefab;

        [Header("Bullet Settings")]
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;
       
        [Header("Space Settings")]
        [Inject]private EntitiesGenerationController _obstaclesGenerationController;

        [Inject]private EnemyDeathCounter _allEnemiesDeathCounter;
        [Inject]private ShipStatisticsController _shipStatisticController;
        [Inject]private EntitiesGenerationData _entitiesGenerationData;
        [Inject]private AnalyticsEventHandler _analyticsEventHandler;
        [Inject]private SpaceShipData _spaceShipData;
        [Inject]private IResourceLoaderService _resourceLoader;
        [Inject]private IInstanceLoader _instanceLoader;
        [Inject]private ISceneLoader _sceneLoader;
        [Inject]private IDeviceInput _deviceInput;
        [Inject]private IAdvertisementService _advertisementService;
        [Inject]private AdvertisementController _advertisingController;
        [Inject]private ShipStatisticsModel _shipStatisticModel;
        [Inject] private IRemoteConfigService _remoteConfigService;
        [Inject]private DataSave _dataForSave;

        private GameOverView ? _endPanelView;
        private SpaceShipController _shipController;
        private ShipStatisticsView _shipStatisticView;
        private Transform _shipTransform;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;

        private void Awake()
        {
            InitializeUI();
            InitializeSpaceShipSystems();
            InitializeEnemySystems();
            InitializeServicesSystems();
        }

        private void Start()
        {
            OnGameStarted?.Invoke();
        }
        private void OnDestroy()
        {
            _obstaclesGenerationController.OnShipSpawned -= ShipInitializedHandler;
            _endPanelView.OnGameReloadClicked -= _sceneLoader.ReloadCurrentScene;
            _obstaclesGenerationController.OnEnemySpawned -= EnemyInitializedHander;
            _shipController.OnPlayerDie -= () => _endPanelView.OnButtonShowAdsClicked-=(_advertisingController.ShowRewardedAdAfterDead);
            _shipController.OnPlayerDie -= () => _endPanelView.OnGameReloadClicked-=(_advertisingController.ShowInterstitialAd);
            _shipController.OnPlayerDie -= () => OnPlayerDied?.Invoke();
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= _obstaclesGenerationController.ReviveShip;
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= _endPanelView.Close;
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= PanelRestartSpawnedHandler;
            _endPanelView.OnButtonGoHomeClicked -= _obstaclesGenerationController.ReloadGameScene;
            _obstaclesGenerationController.OnDestroy();
        }

        private void InitializeUI()
        {
            _shipStatisticView = _resourceLoader.Instantiate(_shipStatisticViewPrefab.gameObject, _UIParent.transform).GetComponent<ShipStatisticsView>();
           
        }

        private void InitializeSpaceShipSystems()
        {
            _obstaclesGenerationController.OnShipSpawned += ShipInitializedHandler;
            _shipStatisticController.Initialize(_shipStatisticModel,_instanceLoader,_dataForSave);
            _entitiesGenerationData.Initialize(_remoteConfigService);
            _obstaclesGenerationController.Initialize(_entitiesGenerationData,_resourceLoader,_instanceLoader,_sceneLoader);
        }

        private void InitializeEnemySystems()
        {
            _obstaclesGenerationController.OnEnemySpawned += EnemyInitializedHander;
        }

        private void InitializeServicesSystems()
        {
            _analyticsEventHandler.Initialize(this, _shipStatisticModel, _weaponShipLaser as LaserWeaponController);
            _advertisingController.Initialize(_advertisementService);
        }

        private void EnemyInitializedHander(EnemyController enemyController, BaseEnemy currentEnemy)
        {
            currentEnemy.OnEnemyDestroyed += EnemyDestroyedHandler;
            currentEnemy.Initialize(_shipTransform,_shipStatisticController);
            enemyController.Initialize(_shipTransform);
        }

        private void EnemyDestroyedHandler(BaseEnemy enemyDestroy)
        {
            _allEnemiesDeathCounter.OnEnemyDied(enemyDestroy);
            enemyDestroy.OnEnemyDestroyed -= EnemyDestroyedHandler;
        }

        private void ShipInitializedHandler(SpaceShipController playerShip)
        {
            _shipTransform = playerShip.transform;
            _shipController = playerShip;
            _weaponController = playerShip.GetComponent<WeaponController>();
            _weaponShipLaser = playerShip.GetComponent<LaserWeaponController>();
            _weaponShipBullet = playerShip.GetComponent<BulletWeaponController>();
            _shipController.OnPlayerDie += () => OnPlayerDied?.Invoke();
            _shipController.OnPlayerDie += () => _advertisingController.OnPlayerRevived += _obstaclesGenerationController.ReviveShip;
            _shipController.OnPlayerDie += PanelRestartSpawnedHandler;
    
            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader,_remoteConfigService);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader,_remoteConfigService);
            _spaceShipData.Initialize(_remoteConfigService);
            _shipController.Initialize(_shipStatisticView, _deviceInput, _shipStatisticController, _weaponShipLaser, _spaceShipData);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform,_remoteConfigService);
        }

        private void PanelRestartSpawnedHandler()
        {

            _endPanelView = _resourceLoader.Instantiate(_restartPrefab, _UIParent).GetComponent<GameOverView>();
            _endPanelView.Initialize();

            _advertisingController.OnPlayerRevived += _endPanelView.Close;
            _endPanelView.OnGameReloadClicked += _sceneLoader.ReloadCurrentScene;
            _shipController.OnPlayerDie += () => _endPanelView.OnGameReloadClicked += (_advertisingController.ShowInterstitialAd);
            _endPanelView.OnButtonGoHomeClicked += _obstaclesGenerationController.ReloadGameScene;

            _endPanelView.OnButtonShowAdsClicked += (_advertisingController.ShowRewardedAdAfterDead);
            _endPanelView.UpdateButtonShowAd(_advertisementService.IsShowed);
            _shipStatisticController.UpdateDestroyedEnemies(_endPanelView);
         
        }

    }
}