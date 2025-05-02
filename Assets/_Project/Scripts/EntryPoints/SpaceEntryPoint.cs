using Asteroid.Enemies;
using Asteroid.Generation;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using System;
using System.Collections;
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
    [SerializeField] private ObstaclesGenerationController _obstaclesGenerationController;
    private ShipStatisticsController _shipStController;
    private ObstaclesGenerationData _obstaclesGenerationData;

    [Header("Ship Systems")]
    [SerializeField] private ShipStatisticsView _shipStView;
    private SpaceShipController _shipController;
    private WeaponController _weaponController;
    private WeaponShip _weaponShipLaser;
    private WeaponShip _weaponShipBullet;

    [Header("UI")]
    [SerializeField] private GameObject _restartPrefab;

    private void Awake()
    {
       _obstaclesGenerationController.Init(OnInitShipSystems);
        InitSpaceSystems();
        InitEnemySystems();
    }
    private void OnInitShipSystems(SpaceShipController playerShip)
    {
        _shipController = playerShip;
        _weaponController = playerShip.GetComponent<WeaponController>();    
        _laserWeaponControl = playerShip.GetComponent<LaserWeaponController>();
        _weaponShipLaser = (WeaponShip)_laserWeaponControl;
        _weaponShipBullet = (WeaponShip)playerShip.GetComponent<BulletWeaponController>();
        //_obstaclesGenerationData = _obstaclesGenerationController.GetComponent<ObstaclesGenerationData>();

        _weaponShipBullet.Init(_bulletPref,_shipStView);
        _weaponShipLaser.Init(_laserPref,_shipStView);
        _laserWeaponControl.Init();
        _shipController.Init(OnPanelRestartSpawned,_shipStView);
        _weaponController.Init(OnBulletSpawned);
        _obstaclesGenerationData.Init(playerShip.transform);
    }
    private void InitSpaceSystems()
    {
       
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
        currentEnemy.Init(_shipStController.ShipStModel);
        enemyController.Init(_shipController.transform);
        if(currentEnemy is AsteroidEnemy aEnemy)
        {
            aEnemy.Init(OnMeteoriteSpawned);
        }
    }
    private void OnMeteoriteSpawned(EnemyController enemyController,BaseEnemy currentEnemy)
    {
        MeteoriteEnemy aster = currentEnemy as MeteoriteEnemy;
        aster.Init(_shipStController.ShipStModel);
        enemyController.Init(_shipController.transform);
    }
    private ShipStatisticsController OnPanelRestartSpawned()
    {
        _shipStController = Instantiate(_restartPrefab).GetComponent<ShipStatisticsController>();
        _shipStController.Init(_shipStView);
        _shipStView = _shipStController.ShipStView;
        _shipController.Init(_shipStController);
        _weaponShipLaser.Init(_bulletPref,_shipStController.ShipStView);
        _weaponShipBullet.Init(_bulletPref,_shipStController.ShipStView);
        return _shipStController;
    }

}

