using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.Services;
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

        [Header("Bullet Settings")]
        [SerializeField] private float _fireBulletSpeed = 15f;
        [SerializeField] private float _fireBulletDamage = 10f;
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;

        [Header("Space Settings")]
        [Inject]private EntitiesGenerationController _obstaclesGenerationController;

        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;
        [SerializeField] private RectTransform _UIParent;
        [SerializeField] private ShipStatisticsView _shipStatisticViewPrefab;
        private ShipStatisticsView _shipStatisticView;

        private LaserWeaponController _laserWeaponControl;
        private Transform _shipTransform;
        [Inject]private EnemyDeathCounter _allEnemiesDeathCounter;
        [Inject]private ShipStatisticsController _shipStatisticController;
        private SpaceShipController _shipController;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;
        [Inject]private EntitiesGenerationData _entitiesGenerationData;
        [Inject]private ShipStatisticsModel _shipStatisticModel;
        [Inject]private SpaceShipData _spaceShipData;
        [Inject]private IResourceLoaderService _resourceLoader;
        [Inject]private IInstanceLoader _instanceLoader;
        [Inject]private ISceneLoader _sceneLoader;
        [Inject]private IDeviceInput _deviceInput;
        [Inject]private AnalyticsEventHandler _analyticsEventHandler;

        private void Awake()
        {
            _shipStatisticView = _resourceLoader.Instantiate(_shipStatisticViewPrefab.gameObject, _UIParent.transform).GetComponent<ShipStatisticsView>();
            InitializeSpaceShipSystems();
            InitializeEnemySystems();

            _analyticsEventHandler.Initialize(this, _shipStatisticModel, _laserWeaponControl);
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

            _allEnemiesDeathCounter.Initialize(_shipStatisticModel);

        }

        private void BulletSpawnedHandler(BaseBullet bullet, Vector2 direction)
        {
            bullet.Initialize(direction, _fireBulletSpeed, _fireBulletDamage);
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
            _laserWeaponControl = playerShip.GetComponent<LaserWeaponController>();
            _weaponShipLaser = _laserWeaponControl;
            _weaponShipBullet = playerShip.GetComponent<BulletWeaponController>();

            _shipController.OnPlayerDie += PanelRestartSpawnedHandler;
            _shipController.OnPlayerDie += () => OnPlayerDied?.Invoke();
            _weaponShipBullet.OnMissalSpawned += BulletSpawnedHandler;

            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView,_shipStatisticController,_resourceLoader);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView, _shipStatisticController,_resourceLoader);
            _shipController.Initialize(_shipStatisticView,_deviceInput,_shipStatisticController,_laserWeaponControl,_resourceLoader,_spaceShipData);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform);
        }
        private void PanelRestartSpawnedHandler()
        {
            var endPanelView = _resourceLoader.Instantiate
                (
                _restartPrefab,
                _shipStatisticView.transform.parent
                ).GetComponent<GameOverView>();

            _shipStatisticView.Initialize(endPanelView,_sceneLoader);
            _shipStatisticController.Initialize();

        }
    }
}