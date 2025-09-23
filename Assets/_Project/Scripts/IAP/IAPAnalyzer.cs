using Asteroid.Database;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Asteroid.Services.IAP
{
    public class IAPAnalyzer: IDisposable, IPurchasingService
    {
        private const string NO_ADS_ID = "NO ADS";
        private const string COINS_100_ID = "COINS 100";
        private const short ADDED_100_COINS = 100;

        private StoreController _storeController;
        private CatalogProvider _catalog;
        private BootstrapUI _bootstrapUI;
        private DataSave    _dataSave;

        public UniTask Initialize(BootstrapUI bootstrapUI, DataSave dataSave)
        {
            _bootstrapUI = bootstrapUI;
            _storeController = UnityIAPServices.StoreController();
            _catalog = new CatalogProvider();
            _dataSave = dataSave;

            _bootstrapUI.OnPlayerClickBuyNoAds += BuyNoAds;
            _bootstrapUI.OnPlayerClickBuy100Coins += Buy100Coins;
            _storeController.OnPurchasePending += OnPurchasePendingHandler;
            _storeController.OnProductsFetched += OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed += OnProductsFailedHandler;
            _storeController.OnStoreDisconnected += OnStroreDisconnectedHandler;
            _storeController.OnPurchaseFailed += OnPurchaseFailedHandler;
            _storeController.OnPurchaseConfirmed += OnPurchasesConfirmedHandler;

            var unityCatalog = ProductCatalog.LoadDefaultCatalog();
            foreach (var item in unityCatalog.allProducts)
            {
                var defaultId = item.id;
                var productType = item.type;
                _catalog.AddProduct(defaultId, productType);
            }
            _catalog.FetchProducts(UnityIAPServices.DefaultProduct().FetchProductsWithNoRetries);
            return _storeController.Connect().AsUniTask();
        }

        public void Buy100Coins()
        { 
            BuyProduct(COINS_100_ID);
        }

        public void BuyNoAds()
        { 
            BuyProduct(NO_ADS_ID);
        }

        public void Dispose()
        {
            _storeController.OnProductsFetched -= OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed -= OnProductsFailedHandler;
            _storeController.OnStoreDisconnected -= OnStroreDisconnectedHandler;
            _storeController.OnPurchaseFailed -= OnPurchaseFailedHandler;
            _storeController.OnPurchasePending -= OnPurchasePendingHandler;
            _storeController.OnPurchaseConfirmed -= OnPurchasesConfirmedHandler;
            _bootstrapUI.OnPlayerClickBuyNoAds -= BuyNoAds;
        }

        private void BuyProduct(string productId)
        {
            var product = _storeController.GetProducts().ToList().Find(p => p.definition.id == productId);
            if (product != null)
            {
                _storeController.PurchaseProduct(product);
            }
            else
            {
                Debug.LogError($"Product {productId} not found!");
            }
        }

        private void OnStroreDisconnectedHandler(StoreConnectionFailureDescription description)
        {
            Debug.Log("Disconnected "+ description.message);
        }

        private void OnProductsFailedHandler(ProductFetchFailed failed)
        {
            Debug.Log("FAILED");
        }

        private void OnPurchasePendingHandler(PendingOrder order)
        {
            Debug.Log("PENDING");
            bool pendedAdvertisements = order.CartOrdered.Items().Contains(order.CartOrdered.Items().Where(predicate => predicate.Product.definition.id == NO_ADS_ID).FirstOrDefault());
            bool pendedAdd100Coins = order.CartOrdered.Items().Contains(order.CartOrdered.Items().Where(predicate => predicate.Product.definition.id == COINS_100_ID).FirstOrDefault());
           
            if (order != null && pendedAdvertisements)
            {
                _dataSave.AdsDisabled = true;
                Debug.ClearDeveloperConsole();
                Debug.Log("������� �������: "+_dataSave.AdsDisabled);
            }

            else if (order != null && pendedAdd100Coins)
            {
                _dataSave.CountCoins += ADDED_100_COINS;
                _bootstrapUI.UpdaaeCountCoins(_dataSave.CountCoins);
            }
            _storeController.ConfirmPurchase(order);
        }

        private void OnProductsFetchedHandler(List<Product> products)
        {
            Debug.Log("Products fetched successfully!");
            foreach (var product in products)
            {
                Debug.Log($"Product: {product.definition.id}, Price: {product.metadata.localizedPrice}");
            }

            _storeController.FetchPurchases();
        }

        private void OnPurchasesConfirmedHandler(Order order)
        {
            Debug.Log("Purchases Confirmed!");
          
            foreach (var product in order.CartOrdered.Items())
                {
                    Debug.Log($"Order: {product.Product.definition.id}, Status: Confirmed ");
                }
        }

        private void OnPurchaseFailedHandler(FailedOrder failedOrder)
        {
            Debug.LogError($"Purchase failed: {failedOrder.FailureReason}, Details: {failedOrder.Details}");
        }


    }
}