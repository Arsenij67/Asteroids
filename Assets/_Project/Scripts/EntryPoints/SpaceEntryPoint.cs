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
using Cysharp.Threading.Tasks;
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

        private event Action<EnemyController, BaseEnemy> _enemyInitializedHandler;
        private event Action<BaseEnemy> _enemyDestroyedHandler;
        private event Action _shipDieHandler;
        private event Action _panelRestartHandler;
        private event Action _onPlayerDiedAction;
        private event Action _showInterstitialAction;
        private event Action _reviveShipAction;
        private event Action _closePanelAction;
        private event Action _reloadSceneAction;
        private event Action _loadMainMenuAction;
        private event Action _showAdsAfterDeadAction;

        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;
        [SerializeField] private RectTransform _UIParent;
        [SerializeField] private ShipStatisticsView _shipStatisticViewPrefab;
        [SerializeField] private GameObject _saveModeUIPrefab;

        [Header("Bullet Settings")]
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;

        [Header("Space Settings")]
        [Inject] private EntitiesGenerationPresenter _obstaclesGenerationController;
        [Inject] private EnemyDeathCounter _allEnemiesDeathCounter;
        [Inject] private GameOverPresenter _shipStatisticPresenter;
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
        [Inject] private SaveDataStrategyController _saveDataStrategyController;
        [Inject] private LocalSaveStrategyPresenter _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private LocalSaveMetaData _localSaveMetaData;

        private GameOverView _endPanelView;
        private SpaceShipPresenter _shipController;
        private ShipStatisticsView _shipStatisticView;
        private Transform _shipTransform;
        private WeaponController _weaponController;
        private WeaponShip _weaponShipLaser;
        private WeaponShip _weaponShipBullet;

        private async void Awake()
        {
            InitializeUI();
            InitializeSpaceShipSystems();
            InitializeEnemySystems();
            await InitializeServicesSystems();
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
            _shipStatisticPresenter.Initialize(_shipStatisticModel, _instanceLoader);
            _entitiesGenerationData.Initialize(_remoteConfigService);
            _obstaclesGenerationController.Initialize(_entitiesGenerationData, _resourceLoader, _instanceLoader, _sceneLoader);
        }

        private void InitializeEnemySystems()
        {
            _enemyInitializedHandler = EnemyInitializedHandler;
            _obstaclesGenerationController.OnEnemySpawned += _enemyInitializedHandler;
        }

        private async UniTask InitializeServicesSystems()
        {
            _analyticsEventHandler.Initialize(this, _shipStatisticModel, _weaponShipLaser as LaserWeaponController);
            _advertisingController.Initialize(_advertisementService);
            await _localSaveStrategy.Initialize(_dataForSave,_localSaveMetaData, _instanceLoader, shipStatisticsPresenter: _shipStatisticPresenter);
            await _cloudSaveStrategy.Initialize(_dataForSave, _instanceLoader, _remoteSave,shipStatisticsPresenter:_shipStatisticPresenter);
            await _saveDataStrategyController.Initialize(_instanceLoader,_resourceLoader,_saveModeUIPrefab,_UIParent,_cloudSaveStrategy, _localSaveStrategy);
        }

        private void EnemyInitializedHandler(EnemyController enemyController, BaseEnemy currentEnemy)
        {
            _enemyDestroyedHandler = (enemy) => EnemyDestroyedHandler(enemy);
            currentEnemy.OnEnemyDestroyed += _enemyDestroyedHandler;
            currentEnemy.Initialize(_shipTransform, _shipStatisticPresenter);
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
            _reviveShipAction = _obstaclesGenerationController.ReviveShip;
            _panelRestartHandler = PanelRestartSpawnedHandler;

            _shipDieHandler = null;
            _shipDieHandler += _onPlayerDiedAction;
            _shipDieHandler += () => _advertisingController.OnPlayerRevived += _reviveShipAction;
            _shipDieHandler += _panelRestartHandler;

            _shipController.OnPlayerDie += _shipDieHandler;

            _weaponShipBullet.Initialize(_bulletPrefab, _shipStatisticView, _shipStatisticPresenter, _resourceLoader, _remoteConfigService);
            _weaponShipLaser.Initialize(_laserPrefab, _shipStatisticView, _shipStatisticPresenter, _resourceLoader, _remoteConfigService);
            _spaceShipData.Initialize(_remoteConfigService);
            _shipController.Initialize(_shipStatisticView, _deviceInput, _shipStatisticPresenter, _weaponShipLaser, _spaceShipData);
            _weaponController.Initialize();
            _entitiesGenerationData.Initialize(_shipTransform, _remoteConfigService);
        }

        private async void PanelRestartSpawnedHandler()
        {
            _shipStatisticPresenter.OpenPanelRestart();
            await _saveDataStrategyController.UpdateDestroyedEnemies(_shipStatisticModel.CountEnemiesDestroyed);
        }
    }
}