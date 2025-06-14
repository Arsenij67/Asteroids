using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using UnityEngine;

namespace Asteroid.Generation
{
    public class SpaceEntryPoint : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] private float _fireBulletSpeed = 15f;
        [SerializeField] private float _fireBulletDamage = 10f;
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;

        [Header("Space Settings")]
        private EntitiesGenerationController _obstaclesGenerationController;

        [Header("Statistic")]
        [SerializeField] private ShipStatisticsView _shipStatisticView;

        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;

        private LaserWeaponController _laserWeaponControl;
        private EnemyDeathCounter _allEnemiesDeathCounter;
        private EntitiesGenerationData _entitiesGenerationData;
        private ShipStatisticsController _shipStatisticController;
        private ShipStatisticsModel _shipStatisticModel;
        private SpaceShipController _shipController;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;
        private Transform _shipTransform;
        private IResourceLoaderService _baseResourceLoader;
        private ISceneLoader _simpleSceneLoader;

        private void Awake()
        {   
            _baseResourceLoader = new BaseResourceLoaderService();

            _entitiesGenerationData = _baseResourceLoader.LoadResource<EntitiesGenerationData>("ScriptableObjects/EntitiesGenerationData");
            _shipStatisticModel = _baseResourceLoader.CreateInstance<ShipStatisticsModel>();
            _shipStatisticController = _baseResourceLoader.CreateInstance<ShipStatisticsController>();
            _allEnemiesDeathCounter = _baseResourceLoader.CreateInstance<EnemyDeathCounter>();
            _obstaclesGenerationController = _baseResourceLoader.CreateInstance<EntitiesGenerationController>();
            _simpleSceneLoader = _baseResourceLoader.CreateInstance<SimpleSceneLoader>();   

            InitializeSpaceShipSystems();
            InitializeEnemySystems();

        }   
        private void OnDestroy()
        {
            _obstaclesGenerationController.OnShipSpawned -= ShipInitializedHandler;
            _shipStatisticView.OnGameReloadClicked -= _simpleSceneLoader.ReloadScene;
            _obstaclesGenerationController.OnEnemySpawned -= EnemyInitializedHander;
            _shipController.OnEnemyDie -= PanelRestartSpawnedHandler;
            _weaponShipBullet.OnMissalSpawned -= BulletSpawnedHandler;

            _obstaclesGenerationController.OnDestroy();
        }
        private void InitializeSpaceShipSystems()
        {
            _obstaclesGenerationController.OnShipSpawned += ShipInitializedHandler;
            _shipStatisticView.OnGameReloadClicked += _simpleSceneLoader.ReloadScene;

            _shipStatisticController.Initialize(_shipStatisticView, _shipStatisticModel);
            _obstaclesGenerationController.Initialize(_entitiesGenerationData,_baseResourceLoader);
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

            currentEnemy.Initialize(_shipTransform);
            enemyController.Initialize(_shipTransform);
        }

        private void EnemyDestroyedHandler(BaseEnemy enemyDestroy)
        {
            _allEnemiesDeathCounter.OnEnemyDied();
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

            _shipController.OnEnemyDie += PanelRestartSpawnedHandler;
            _weaponShipBullet.OnMissalSpawned += BulletSpawnedHandler;
            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView,_baseResourceLoader);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView,_baseResourceLoader);
            _shipController.Initialize(_shipStatisticView,new DesktopInput(),_shipStatisticController,_laserWeaponControl,_baseResourceLoader);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform);
        }
        private void PanelRestartSpawnedHandler()
        {
            var endPanelView = _baseResourceLoader.Instantiate
                (
                _restartPrefab,
                _shipStatisticView.transform.parent
                ).GetComponent<GameOverView>();

            _shipStatisticView.Initialize(endPanelView);
            _shipStatisticController.Initialize();
        }
    }
}