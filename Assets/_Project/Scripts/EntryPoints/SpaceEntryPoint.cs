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
        [SerializeField] private GameObject _laserPrefab;
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Space Settings")]
        [SerializeField] private EntitiesGenerationController _obstaclesGenerationController;

        [Header("Statistic")]
        [SerializeField] private ShipStatisticsView _shipStatisticView;

        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;

        private LaserWeaponController _laserWeaponControl;
        private EntitiesGenerationData _entitiesGenerationData;
        private ShipStatisticsController _shipStatisticController;
        private ShipStatisticsModel _shipStatisticModel;
        private SpaceShipController _shipController;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;
        private Transform _shipTransform;

        private IResourceLoaderService _resourceLoader;
        private void Awake()
        {
            _resourceLoader = new BaseResourceLoaderService();

            _entitiesGenerationData = _resourceLoader.LoadResource<EntitiesGenerationData>("ScriptableObjects/EntitiesGenerationData");
            _shipStatisticModel = _resourceLoader.CreateInstance<ShipStatisticsModel>();
            _shipStatisticController = _resourceLoader.CreateInstance<ShipStatisticsController>();

            InitializeSpaceShipSystems();
            InitializeEnemySystems();
        }
        private void OnDestroy()
        {
            _shipStatisticController?.RemoveAllListeners();
        }
        private void InitializeSpaceShipSystems()
        {
            _shipStatisticController.Initialize(_shipStatisticView, _shipStatisticModel);
            _obstaclesGenerationController.Initialize(OnInitializedShipSystems, _entitiesGenerationData);
        }
        private void InitializeEnemySystems()
        {
            _obstaclesGenerationController.Initialize(OnEnemySpawned);
        }
        private void OnBulletSpawned(FireballBullet bullet, Vector2 direction)
        {
            bullet.Initialize(direction, _fireBulletSpeed, _fireBulletDamage);
        }

        private void OnEnemySpawned(EnemyController enemyController, BaseEnemy currentEnemy)
        {
            currentEnemy.Initialize(_shipStatisticModel);
            enemyController.Initialize(_shipTransform);

            if (currentEnemy is AsteroidEnemy asteroidEnemy)
            {
                asteroidEnemy.Initialize(OnMeteoriteSpawned, _shipTransform.position);
            }
        }
        private void OnMeteoriteSpawned(EnemyController enemyController, BaseEnemy currentEnemy)
        {
            if (currentEnemy is MeteoriteEnemy meteorite)
            {
                enemyController.Initialize(_shipController?.transform);
                meteorite.Initialize(_shipStatisticModel);
            }
        }
        private void OnInitializedShipSystems(SpaceShipController playerShip)
        {
            _shipTransform = playerShip.transform;
            _shipController = playerShip;
            _weaponController = playerShip.GetComponent<WeaponController>();
            _laserWeaponControl = playerShip.GetComponent<LaserWeaponController>();
            _weaponShipLaser = (WeaponShip)_laserWeaponControl;
            _weaponShipBullet = (WeaponShip)playerShip.GetComponent<BulletWeaponController>();

            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView);
            _laserWeaponControl.Initialize();

            _shipController.Initialize(
                OnPanelRestartSpawned,
                _shipStatisticView,
                new DesktopInput(),
                _shipStatisticController);

            _weaponController.Initialize(OnBulletSpawned);
            _entitiesGenerationData.Initialize(_shipTransform);
        }
        private void OnPanelRestartSpawned()
        {
            var endPanelView = _resourceLoader.Instantiate(
                _restartPrefab,
                _shipStatisticView.transform.parent)
                .GetComponent<GameOverView>();

            _shipStatisticView.Initialize(endPanelView);
            _shipStatisticController.Initialize();
        }
    }
}