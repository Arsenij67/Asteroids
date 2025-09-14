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
    public class IAPAnalyzer: IDisposable
    {
        private const string NO_ADS_ID= "NO ADS";
        private const string COINS_100 = "COINS 100";

        private StoreController _storeController;
        private CatalogProvider _catalog;
        private BootstrapUI _bootstrapUI;
        private DataSave    _dataSave;

        public async UniTask Initialize(BootstrapUI bootstrapUI, DataSave dataSave)
        {
            _bootstrapUI = bootstrapUI;
            _storeController = UnityIAPServices.StoreController();
            _catalog = new CatalogProvider();
            _dataSave = dataSave;
            _bootstrapUI.OnPlayerClickBuyNoAds += BuyNoAds;

            _storeController.OnPurchasePending += OnPurchasePendingHandler;
            _storeController.OnProductsFetched += OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed += OnProductsFailedHandler;
            _storeController.OnPurchasesFetched += OnPurchasesFetchedHandler;
            _storeController.OnStoreDisconnected += OnStroreDisconnectedHandler;
            _storeController.OnPurchaseFailed += OnPurchaseFailedHandler;

            await _storeController.Connect();

            _catalog.AddProduct(NO_ADS_ID, ProductType.NonConsumable);
            _catalog.AddProduct(COINS_100, ProductType.Consumable);
            _catalog.FetchProducts(UnityIAPServices.DefaultProduct().FetchProductsWithNoRetries);
        }

        public void BuyNoAds()
        { 
            BuyProduct(NO_ADS_ID);
        }
        public void Dispose()
        {
            _storeController.OnProductsFetched -= OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed -= OnProductsFailedHandler;
            _storeController.OnPurchasesFetched -= OnPurchasesFetchedHandler;
            _storeController.OnStoreDisconnected -= OnStroreDisconnectedHandler;
            _storeController.OnPurchaseFailed -= OnPurchaseFailedHandler;
            _storeController.OnPurchasePending -= OnPurchasePendingHandler;
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

        private void OnPurchasePendingHandler(PendingOrder order) // Покупка была оплачена, но еще не выполнена. вызывается, когда покупка совершена и ожидает подтверждения.

        {
            Debug.Log("PENDING");
            // тут логика разблокировки покупки
            _storeController.ConfirmPurchase(order);
        
        }

        void OnProductsFetchedHandler(List<Product> products)
        {
            Debug.Log("Products fetched successfully!");
            foreach (var product in products)
            {
                Debug.Log($"Product: {product.definition.id}, Price: {product.metadata.localizedPrice}");
            }

            _storeController.FetchPurchases();
        }

        private void OnPurchasesFetchedHandler(Orders orders)
        {
            Debug.Log("Purchases fetched!");
            foreach (var order in orders.ConfirmedOrders)
            {
                foreach (var product in order.CartOrdered.Items())
                {
                    Debug.Log($"Order: {product.Product.definition.id}, Status: Confirmed");
                }
            }
        }

        private void OnPurchaseFailedHandler(FailedOrder failedOrder)
        {
            Debug.LogError($"Purchase failed: {failedOrder.FailureReason}, Details: {failedOrder.Details}");
        }


    }
}