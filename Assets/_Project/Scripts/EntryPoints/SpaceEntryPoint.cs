using Asteroid.Database;
using Asteroid.Enemies;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.Services.UnityCloud;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.Weapon;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class SpaceEntryPoint : MonoBehaviour
    {
        public event Action OnGameStarted;
        public event Action OnPlayerDied;

        private Action<EnemyController, BaseEnemy> _enemyInitializedHandler;
        private Action<BaseEnemy> _enemyDestroyedHandler;
        private Action _shipDieHandler;
        private Action _panelRestartHandler;
        private Action _onPlayerDiedAction;
        private Action _showInterstitialAction;
        private Action _reviveShipAction;
        private Action _closePanelAction;
        private Action _reloadSceneAction;
        private Action _loadMainMenuAction;
        private Action _showAdsAfterDeadAction;

        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;
        [SerializeField] private RectTransform _UIParent;
        [SerializeField] private ShipStatisticsView _shipStatisticViewPrefab;

        [Header("Bullet Settings")]
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;

        [Header("Space Settings")]
        [Inject] private EntitiesGenerationPresenter _obstaclesGenerationController;
        [Inject] private EnemyDeathCounter _allEnemiesDeathCounter;
        [Inject] private ShipStatisticsPresenter _shipStatisticController;
        [Inject] private EntitiesGenerationData _entitiesGenerationData;
        [Inject] private AnalyticsEventHandler _analyticsEventHandler;
        [Inject] private SpaceShipData _spaceShipData;
        [Inject] private IResourceLoaderService _resourceLoader;
        [Inject] private IInstanceLoader _instanceLoader;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IDeviceInput _deviceInput;
        [Inject] private IAdvertisementService _advertisementService;
        [Inject] private AdvertisementPresenter _advertisingController;
        [Inject] private ShipStatisticsModel _shipStatisticModel;
        [Inject] private IRemoteConfigService _remoteConfigService;
        [Inject] private DataSave _dataForSave;
        [Inject] private IRemoteSavable _remoteSave;
        [Inject] private CloudDataPresenter _cloudController;

        private GameOverView _endPanelView;
        private SpaceShipPresenter _shipController;
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
            UnsubscribeFromAllEvents();
        }

        private void UnsubscribeFromAllEvents()
        {
            if (_obstaclesGenerationController != null)
            {
                _obstaclesGenerationController.OnShipSpawned -= ShipInitializedHandler;
                _obstaclesGenerationController.OnEnemySpawned -= EnemyInitializedHandler;
            }

            if (_endPanelView != null)
            {
                _endPanelView.OnGameReloadClicked -= _reloadSceneAction;
                _endPanelView.OnButtonGoHomeClicked -= _loadMainMenuAction;
                _endPanelView.OnButtonShowAdsClicked -= _showAdsAfterDeadAction;
                _endPanelView.OnGameReloadClicked -= _showInterstitialAction;
            }

            if (_shipController != null)
            {
                _shipController.OnPlayerDie -= _shipDieHandler;
            }

            if (_advertisingController != null)
            {
                _advertisingController.OnPlayerRevived -= _reviveShipAction;
                _advertisingController.OnPlayerRevived -= _closePanelAction;
            }

            _obstaclesGenerationController?.OnDestroy();
        }

        private void InitializeUI()
        {
            _shipStatisticView = _resourceLoader.Instantiate(_shipStatisticViewPrefab.gameObject, _UIParent.transform)
                .GetComponent<ShipStatisticsView>();
        }

        private void InitializeSpaceShipSystems()
        {
            _obstaclesGenerationController.OnShipSpawned += ShipInitializedHandler;
            _shipStatisticController.Initialize(_shipStatisticModel, _instanceLoader, _dataForSave);
            _entitiesGenerationData.Initialize(_remoteConfigService);
            _obstaclesGenerationController.Initialize(_entitiesGenerationData, _resourceLoader, _instanceLoader, _sceneLoader);
        }

        private void InitializeEnemySystems()
        {
            _enemyInitializedHandler = EnemyInitializedHandler;
            _obstaclesGenerationController.OnEnemySpawned += _enemyInitializedHandler;
        }

        private void InitializeServicesSystems()
        {
            _analyticsEventHandler.Initialize(this, _shipStatisticModel, _weaponShipLaser as LaserWeaponController);
            _advertisingController.Initialize(_advertisementService);
            _cloudController.Initialize(_dataForSave,remoteSavable:_remoteSave);
        }

        private void EnemyInitializedHandler(EnemyController enemyController, BaseEnemy currentEnemy)
        {
            _enemyDestroyedHandler = (enemy) => EnemyDestroyedHandler(enemy);
            currentEnemy.OnEnemyDestroyed += _enemyDestroyedHandler;
            currentEnemy.Initialize(_shipTransform, _shipStatisticController);
            enemyController.Initialize(_shipTransform);
        }

        private void EnemyDestroyedHandler(BaseEnemy enemyDestroy)
        {
            _allEnemiesDeathCounter.OnEnemyDied(enemyDestroy);
            enemyDestroy.OnEnemyDestroyed -= _enemyDestroyedHandler;
        }

        private void ShipInitializedHandler(SpaceShipPresenter playerShip)
        {
            _shipTransform = playerShip.transform;
            _shipController = playerShip;
            _weaponController = playerShip.GetComponent<WeaponController>();
            _weaponShipLaser = playerShip.GetComponent<LaserWeaponController>();
            _weaponShipBullet = playerShip.GetComponent<BulletWeaponController>();

            _onPlayerDiedAction = () => OnPlayerDied?.Invoke();
            _reviveShipAction = () => _obstaclesGenerationController.ReviveShip();
            _panelRestartHandler = PanelRestartSpawnedHandler;

            _shipDieHandler = null;
            _shipDieHandler += _onPlayerDiedAction;
            _shipDieHandler += () => _advertisingController.OnPlayerRevived += _reviveShipAction;
            _shipDieHandler += _panelRestartHandler;

            _shipController.OnPlayerDie += _shipDieHandler;

            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader, _remoteConfigService);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView, _shipStatisticController, _resourceLoader, _remoteConfigService);
            _spaceShipData.Initialize(_remoteConfigService);
            _shipController.Initialize(_shipStatisticView, _deviceInput, _shipStatisticController, _weaponShipLaser, _spaceShipData);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform, _remoteConfigService);
        }

        private void PanelRestartSpawnedHandler()
        {
            _endPanelView = _resourceLoader.Instantiate(_restartPrefab, _UIParent).GetComponent<GameOverView>();
            _endPanelView.Initialize();

            _closePanelAction = () => _endPanelView.Close();
            _reloadSceneAction = () => _sceneLoader.ReloadCurrentScene();
            _loadMainMenuAction = () => _obstaclesGenerationController.LoadMainMenuScene();
            _showAdsAfterDeadAction = () => _advertisingController.ShowRewardedAdAfterDead();
            _showInterstitialAction = () => _advertisingController.ShowInterstitialAdBeforeRestart();

            _advertisingController.OnPlayerRevived += _closePanelAction;
            _endPanelView.OnGameReloadClicked += _reloadSceneAction;
            _endPanelView.OnButtonGoHomeClicked += _loadMainMenuAction;
            _endPanelView.OnButtonShowAdsClicked += _showAdsAfterDeadAction;
            _endPanelView.OnGameReloadClicked += _showInterstitialAction;

            _endPanelView.UpdateButtonShowAd(_advertisementService.IsShowed);
            _shipStatisticController.UpdateDestroyedEnemies(_endPanelView);
            _cloudController.AddCountDeadEnemies(_shipStatisticModel.CountEnemiesDestroyed);
        }
    }
}