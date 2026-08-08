using Asteroid.Database;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.Services.UnityCloud;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Asteroid.UI;
using Asteroid.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class SpaceEntryPoint : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _restartPrefab;
        [SerializeField] private RectTransform _UIParent;
        [SerializeField] private ShipStatisticsView _shipStatisticViewPrefab;
        [SerializeField] private SaveModeUI _saveModeUIPrefab;

        [Header("Bullet Settings")]
        [SerializeField] private LaserBullet _laserPrefab;
        [SerializeField] private FireballBullet _bulletPrefab;

        [Header("Space Settings")]
        [Inject] private EntitiesGenerationFactory _entitiesGenerationFactory;
        [Inject] private EnemyDeathCounter _allEnemiesDeathCounter;
        [Inject] private GameOverPresenter _gameOverPresenter;
        [Inject] private EntitiesGenerationData _entitiesGenerationData;
        [Inject] private AnalyticsEventHandler _analyticsEventHandler;
        [Inject] private SpaceShipData _spaceShipData;
        [Inject] private IResourceLoader _resourceLoader;
        [Inject] private InstanceCreator _instanceLoader;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IDeviceInput _deviceInput;
        [Inject] private IAdvertisementService _advertisementService;
        [Inject] private AdvertisementPresenter _advertisingPresenter;
        [Inject] private ShipStatisticsModel _shipStatisticModel;
        [Inject] private IRemoteConfigService _remoteConfigService;
        [Inject] private DataSave _dataForSave;
        [Inject] private IAnalytics _analyticsService;
        [Inject] private IRemoteSavable _remoteSave;
        [Inject] private SaveDataStrategyManager _saveDataStrategyController;
        [Inject] private LocalSaveStrategyPresenter _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private LocalSaveMetaData _localSaveMetaData;
        [Inject] private ShipStatisticPresenter _shipStatisticPresenter;
        [Inject] private BootstrapSceneData  _bootstrapSceneData;

        private ShipStatisticsView _shipStatisticView;
        private WeaponShip _weaponShipLaser;

        private async void Awake()
        {
            InitializeUI();
            InitializeSpaceShipSystems();
            await InitializeServicesSystems();
            InitializeEnemySystems();
        }
        
        private void OnDestroy()
        {
            _entitiesGenerationFactory?.OnDestroy();
            _gameOverPresenter?.Dispose();
        }

        private void InitializeUI()
        {
            _shipStatisticView = _resourceLoader.Instantiate<ShipStatisticsView>(_shipStatisticViewPrefab, _UIParent);
            _shipStatisticPresenter.Initialize(_shipStatisticModel, _shipStatisticView);
            _entitiesGenerationData.Initialize(_remoteConfigService);

            _gameOverPresenter.Initialize(
            _resourceLoader,
            _UIParent,
            _entitiesGenerationData.EndPanelView,
           _advertisingPresenter,
           _shipStatisticModel,
           _UIParent,
           _sceneLoader,
           _bootstrapSceneData);
        }

        private void InitializeSpaceShipSystems()
        {
          _entitiesGenerationFactory.Initialize(
          _analyticsEventHandler,
          _advertisingPresenter,
          _entitiesGenerationData,
          _resourceLoader,
          _instanceLoader,
          _allEnemiesDeathCounter,
          _gameOverPresenter,
          _shipStatisticPresenter,
          _spaceShipData,
          _shipStatisticView,
          _remoteConfigService,
          _deviceInput);

          _weaponShipLaser = _entitiesGenerationFactory.CreateShip(_entitiesGenerationData.PlayerShipToGenerateNow).GetComponent<LaserWeaponController>();
          _entitiesGenerationData.Initialize(_remoteConfigService, _weaponShipLaser.transform);
          _analyticsEventHandler.Initialize(_analyticsService, _shipStatisticModel, _weaponShipLaser as LaserWeaponController);
          _entitiesGenerationFactory.SubscribeShip();
        }

        private async UniTask InitializeServicesSystems()
        {
            _advertisingPresenter.Initialize(_advertisementService);
            await _localSaveStrategy.Initialize(_dataForSave, _localSaveMetaData, _instanceLoader, shipStatisticsPresenter: _gameOverPresenter);
            await _cloudSaveStrategy.Initialize(_dataForSave, _instanceLoader, _remoteSave, shipStatisticsPresenter: _gameOverPresenter);
            await _saveDataStrategyController.Initialize(_instanceLoader, _resourceLoader, _saveModeUIPrefab, _UIParent, _cloudSaveStrategy, _localSaveStrategy);
        }

        private void InitializeEnemySystems()
        {
            _entitiesGenerationFactory.StartEnemiesCreation();
        }
    }
}