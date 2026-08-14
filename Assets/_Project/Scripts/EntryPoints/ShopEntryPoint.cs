using Asteroid.Database;
using Asteroid.Database.Connection;
using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Asteroid.EntryPoints
{
    public class ShopEntryPoint : MonoBehaviour
    {
        [Inject] private IPurchasingService _purchaseService;
        [Inject] private InstanceCreator _instanceLoader;
        [Inject] private IRemoteSavable _remoteSavable;
        [Inject] private IResourceLoader _resourceLoaderService;
        [Inject] private LocalSaveStrategy _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private SaveDataStrategyManager _saveDataStrategy;
        [Inject] private DataSave _dataSave;
        [Inject] private LocalSaveMetaData _localSave;
        [Inject] private WIFIConnector _wiFiChecker;

        [SerializeField] private SaveModeUI _saveModeUIPrefab;
        [SerializeField] RectTransform _parentUI;
        [SerializeField] private ShopView _shopUI;

        public Transform tr;

        private async void Start()
        {
          await  ClearCache();
     

            _shopUI.Initialize();
            _wiFiChecker.Initialize(_instanceLoader);
            await _localSaveStrategy.Initialize(_dataSave,_localSave, _instanceLoader,_shopUI);
            await _cloudSaveStrategy.Initialize(_wiFiChecker, _dataSave, _instanceLoader, _remoteSavable, _shopUI);
            await _saveDataStrategy.Initialize(_wiFiChecker,_instanceLoader,_resourceLoaderService,_saveModeUIPrefab,_parentUI,_cloudSaveStrategy, _localSaveStrategy);

 

            _purchaseService.OnPlayerBought100Coins += _saveDataStrategy.AddAndRefreshCoins;
            _purchaseService.OnPlayerBoughtNoAds += _saveDataStrategy.AddAndRefreshAds;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;
        }

        public async UniTask ClearCache()
        {
            if (!Caching.ClearCache())
            {
                Debug.Log("Не удалось очистить кэш");
            }
            else
            {
                Debug.Log("Кэш очищен");

            }
            GameObject[] obj =
            { await Addressables.InstantiateAsync("RedFireball", tr).ToUniTask(),
         await Addressables.InstantiateAsync("Meteorite", tr).ToUniTask(),
         await Addressables.InstantiateAsync("RedLiserPrefab", tr).ToUniTask(),
         await Addressables.InstantiateAsync("spaceship_1", tr).ToUniTask(),
         await Addressables.InstantiateAsync("spaceship_2", tr).ToUniTask()
            };

            foreach (var o in obj)
            {
                o.transform.localScale = Vector2.one * 200;
            }
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= _saveDataStrategy.AddAndRefreshCoins;
            _purchaseService.OnPlayerBoughtNoAds -= _saveDataStrategy.AddAndRefreshAds;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
            _resourceLoaderService.UnloadAllResources();
            _purchaseService.Dispose();
            _saveDataStrategy.Dispose();
        }
    }
}
