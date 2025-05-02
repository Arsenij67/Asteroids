using Asteroid.Enemies;
using Asteroid.Generation;
using Asteroid.Inputs;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using UnityEngine;
public class SpaceEntryPoint : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float _fireBulletSpeed = 15f;
    [SerializeField] private float _fireBulletDamage = 10f;
    [SerializeField] private GameObject _laserPref;
    [SerializeField] private GameObject _bulletPref;
    private LaserWeaponController _laserWeaponControl;

    [Header("Space Settings")]
    [SerializeField] private EntitiesGenerationController _obstaclesGenerationController;
    private EntitiesGenerationData _entitiesGenerationData;

    [Header("Statistic")]
    [SerializeField] private ShipStatisticsView _shipStView;
    private ShipStatisticsController _shipStController;
    private ShipStatisticsModel _shipStModel;

    [Header("Ship Systems")]
    private SpaceShipController? _shipController;
    private WeaponController _weaponController;
    private WeaponShip _weaponShipLaser;
    private WeaponShip _weaponShipBullet;
    private Transform _shipTransform;

    [Header("UI")]
    [SerializeField] private GameObject _restartPrefab;

    private void Awake()
    {
        _entitiesGenerationData = Resources.Load<EntitiesGenerationData>("ScriptableObjects/EntitiesGenerationData");
        _shipStModel = Resources.Load<ShipStatisticsModel>("ScriptableObjects/StatisticsModel");
        _shipStController = new ShipStatisticsController();

        InitSpaceShipSystems();
        InitEnemySystems();
    }
    private void InitSpaceShipSystems()
    {
        _shipStController.Init(_shipStView, _shipStModel);
        _obstaclesGenerationController.Init(OnInitShipSystems, _entitiesGenerationData);
    }
    private void InitEnemySystems()
    {
        _obstaclesGenerationController.Init(OnEnemySpawned);
    }
    private void OnBulletSpawned(FireballBullet bullet, Vector2 direction)
    {
        bullet.Init(direction, _fireBulletSpeed, _fireBulletDamage);
    }    
    private void OnEnemySpawned(EnemyController enemyController, BaseEnemy currentEnemy)
    {
        currentEnemy.Init(_shipStModel);
        enemyController.Init(_shipTransform);
        if(currentEnemy is AsteroidEnemy aEnemy)
        {
            aEnemy.Init(OnMeteoriteSpawned,_shipTransform.position);
        }
    }
    private void OnMeteoriteSpawned(EnemyController enemyController, BaseEnemy currentEnemy)
    {
        MeteoriteEnemy aster = currentEnemy as MeteoriteEnemy;
        enemyController.Init(_shipController?.transform);
        aster.Init(_shipStModel);
    }
    private void OnInitShipSystems(SpaceShipController playerShip)
    {
        _shipTransform = playerShip.transform;
        _shipController = playerShip;
        _weaponController = playerShip.GetComponent<WeaponController>();
        _laserWeaponControl = playerShip.GetComponent<LaserWeaponController>();
        _weaponShipLaser = (WeaponShip)_laserWeaponControl;
        _weaponShipBullet = (WeaponShip)playerShip.GetComponent<BulletWeaponController>();

        _weaponShipBullet.Init(_bulletPref, _shipStView);
        _weaponShipLaser.Init(_laserPref, _shipStView);
        _laserWeaponControl.Init();
        _shipController.Init(OnPanelRestartSpawned, _shipStView, new DesktopInput(),_shipStController);
        _weaponController.Init(OnBulletSpawned);
        _entitiesGenerationData.Init(_shipTransform);
    }
    private void OnPanelRestartSpawned()
    {
        var endPanelView = Instantiate(_restartPrefab, _shipStView.transform.parent).GetComponent<GameOverView>();
        _shipStView.Init(endPanelView);
        _shipStController.Init();
    }

    private void OnDestroy()
    {
        _shipStController?.OnDestroy();
    }

}

