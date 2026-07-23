using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityCloud;
using Asteroid.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.EntryPoints
{
    public class ShopEntryPoint : MonoBehaviour
    {
        [Inject] private ShopUI _shopUI;
        [Inject] private IPurchasingService _purchaseService;
        [Inject] private IInstanceLoader _instanceLoader;
        [Inject] private IRemoteSavable _remoteSavable;
        [Inject] private IResourceLoaderService _resourceLoaderService;
        [Inject] private LocalSaveStrategyPresenter _localSaveStrategy;
        [Inject] private CloudDataPresenter _cloudSaveStrategy;
        [Inject] private SaveDataStrategyController _saveDataStrategy;
        [Inject] private TMP_Text _textCoins;
        [Inject] private DataSave _dataSave;
        [Inject] private LocalSaveMetaData _localSave;
        [Inject] private Image _imageNoAds;
        [Inject(Id = "buttonBuyNoAds")] private Button _buttonBuyNoAds;
        [Inject(Id = "buttonBuy100Coins")] private Button _buttonBuy100Coins;

        [SerializeField] private GameObject _saveModeUIPrefab;
        [SerializeField] RectTransform _parentUI;

        private async void Start()
        {
            _shopUI.Initialize(_buttonBuyNoAds, _buttonBuy100Coins, _textCoins, _imageNoAds);
            await _purchaseService.Initialize(_dataSave);
            await _localSaveStrategy.Initialize(_dataSave,_localSave, _instanceLoader,_shopUI);
            await _saveDataStrategy.Initialize(_instanceLoader,_resourceLoaderService,_saveModeUIPrefab,_parentUI,_cloudSaveStrategy, _localSaveStrategy);
            _cloudSaveStrategy.Initialize(_dataSave, _instanceLoader, _shopUI,_remoteSavable);

            _purchaseService.OnPlayerBought100Coins += _saveDataStrategy.UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins += _saveDataStrategy.UpdateUICountCoins; 
            _purchaseService.OnPlayerBoughtNoAds += _saveDataStrategy.UpdateNoAdsAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds += _saveDataStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins += _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds += _purchaseService.BuyNoAds;

            _shopUI.UpdateViewNoAds(_saveDataStrategy.NoAdsStatus);
            _shopUI.UpdateCountCoins(_saveDataStrategy.CountCoins);
        }

        private void OnDestroy()
        {
            _purchaseService.OnPlayerBought100Coins -= _saveDataStrategy.UpdateCoinsAfterPurchase;
            _purchaseService.OnPlayerBought100Coins -= _saveDataStrategy.UpdateUICountCoins;
            _purchaseService.OnPlayerBoughtNoAds -= _saveDataStrategy.UpdateNoAdsAfterPurchase;
            _purchaseService.OnPlayerBoughtNoAds -= _saveDataStrategy.UpdateUINoAds;
            _shopUI.OnPlayerClickBuy100Coins -= _purchaseService.Buy100Coins;
            _shopUI.OnPlayerClickBuyNoAds -= _purchaseService.BuyNoAds;
        }


    }
}
