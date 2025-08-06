using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.Services;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Advertisements;
using Asteroid.UnityAdvertisement;

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

        private GameOverView ? _endPanelView;
        private SpaceShipController _shipController;
        private ShipStatisticsView _shipStatisticView;
        private Transform _shipTransform;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;

        private void Awake()
        {
      
            _shipStatisticView = _resourceLoader.Instantiate(_shipStatisticViewPrefab.gameObject, _UIParent.transform).GetComponent<ShipStatisticsView>();
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
            _shipStatisticView.OnGameReloadClicked -= _sceneLoader.ReloadScene;
            _obstaclesGenerationController.OnEnemySpawned -= EnemyInitializedHander;
            _shipController.OnPlayerDie -= PanelRestartSpawnedHandler;
            _weaponShipBullet.OnMissalSpawned -= BulletSpawnedHandler;
            _shipController.OnPlayerDie -= () => _endPanelView.ButtonShowAd.onClick.AddListener(_advertisingController.ShowRewardedAdAfterDead);
            _shipController.OnPlayerDie -= () => _endPanelView.ButtonRestart.onClick.AddListener(_advertisingController.ShowInterstitialAd);
            _shipController.OnPlayerDie -= () => OnPlayerDied?.Invoke();
            _weaponShipBullet.OnMissalSpawned -= BulletSpawnedHandler;
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= _obstaclesGenerationController.ReviveShip;
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= _endPanelView.Close;
            _shipController.OnPlayerDie -= () => _advertisingController.OnPlayerRevived -= _endPanelView.DisableButtonShowAd;

            _obstaclesGenerationController.OnDestroy();
        }
        private void InitializeSpaceShipSystems()
        {
            _obstaclesGenerationController.OnShipSpawned += ShipInitializedHandler;
            _shipStatisticView.OnGameReloadClicked += _sceneLoader.ReloadScene;
            _shipStatisticController.Initialize(_shipStatisticView, _shipStatisticModel,_instanceLoader);
            _obstaclesGenerationController.Initialize(_entitiesGenerationData,_resourceLoader,_instanceLoader);
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

        private void BulletSpawnedHandler(BaseBullet bullet, Vector2 direction)
        {
            bullet.Initialize(direction);
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

            _shipController.OnPlayerDie += PanelRestartSpawnedHandler;
            _shipController.OnPlayerDie += () => _endPanelView.ButtonShowAd.onClick.AddListener(_advertisingController.ShowRewardedAdAfterDead);
            _shipController.OnPlayerDie += () => _endPanelView.ButtonRestart.onClick.AddListener(_advertisingController.ShowInterstitialAd);
            _shipController.OnPlayerDie += () => OnPlayerDied?.Invoke();
            _shipController.OnPlayerDie += () => _advertisingController.OnPlayerRevived += _obstaclesGenerationController.ReviveShip;
            _shipController.OnPlayerDie += () => _advertisingController.OnPlayerRevived += _endPanelView.Close;
            _shipController.OnPlayerDie += () => _advertisingController.OnPlayerRevived += _endPanelView.DisableButtonShowAd;
            _weaponShipBullet.OnMissalSpawned += BulletSpawnedHandler;

            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader);
            _shipController.Initialize(_shipStatisticView, _deviceInput, _shipStatisticController, _weaponShipLaser, _resourceLoader, _spaceShipData);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform);
        }
                                                                
        private void PanelRestartSpawnedHandler()
        {
            _endPanelView = _resourceLoader.Instantiate
                (
                _restartPrefab,
                _shipStatisticView.transform.parent
                ).GetComponent<GameOverView>();

            _endPanelView.Open();
            _shipStatisticView.Initialize(_endPanelView,_sceneLoader);
            _shipStatisticController.Initialize();
        }
    }
}