using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.EntryPoints
{
    public class ShopEntryPoint : MonoBehaviour
    {
        [Inject] private IPurchasingService _purchaseService;
        [Inject] private InstanceCreator _instanceLoader;
        [Inject] private IRemoteSavable _remoteSavable;
        [Inject] private IResourceLoader _resourceLoaderService;
        [Inject] private LocalSaveStrategyPresenter _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private SaveDataStrategyManager _saveDataStrategy;
        [Inject] private DataSave _dataSave;
        [Inject] private LocalSaveMetaData _localSave;

        [SerializeField] private SaveModeUI _saveModeUIPrefab;
        [SerializeField] RectTransform _parentUI;
        [SerializeField] private ShopView _shopUI;

        private async void Start()
        {
            _shopUI.Initialize();
            await _purchaseService.Initialize(_dataSave);
            await _localSaveStrategy.Initialize(_dataSave,_localSave, _instanceLoader,_shopUI);
            await _cloudSaveStrategy.Initialize(_dataSave, _instanceLoader, _remoteSavable, _shopUI);
            await _saveDataStrategy.Initialize(_instanceLoader,_resourceLoaderService,_saveModeUIPrefab,_parentUI,_cloudSaveStrategy, _localSaveStrategy);
    
            _purchaseService.OnPlayerBought100Coins += _saveDataStrategy.UpdateCoins;
            _purchaseService.OnPlayerBoughtNoAds += _saveDataStrategy.UpdateNoAds;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= _saveDataStrategy.UpdateCoins;
            _purchaseService.OnPlayerBoughtNoAds -= _saveDataStrategy.UpdateNoAds;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        }
    }
}
