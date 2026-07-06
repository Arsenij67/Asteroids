using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;

namespace Asteroid.Services.IAP
{
    public class IAPAnalyzer : IDisposable, IPurchasingService, IInitializable
    {
        public event UnityAction OnPlayerBought100Coins;
        public event UnityAction OnPlayerBoughtNoAds;

        private const string NO_ADS_ID = "NO ADS";
        private const string COINS_100_ID = "COINS 100";

        private StoreController _storeController;
        private CatalogProvider _catalog;

        public void Initialize()
        {
            _storeController = UnityIAPServices.StoreController();

            _storeController.OnPurchasePending += OnPurchasePendingHandler;
            _storeController.OnProductsFetched += OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed += OnProductsFailedHandler;
            _storeController.OnStoreDisconnected += OnStoreDisconnectedHandler;
            _storeController.OnPurchaseFailed += OnPurchaseFailedHandler;
            _storeController.OnPurchaseConfirmed += OnPurchasesConfirmedHandler;
        }

        public UniTask Initialize(DataSave dataSave)
        {
            _catalog = new CatalogProvider();
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
            _storeController.OnStoreDisconnected -= OnStoreDisconnectedHandler;
            _storeController.OnPurchaseFailed -= OnPurchaseFailedHandler;
            _storeController.OnPurchasePending -= OnPurchasePendingHandler;
            _storeController.OnPurchaseConfirmed -= OnPurchasesConfirmedHandler;
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

        private void OnStoreDisconnectedHandler(StoreConnectionFailureDescription description)
        {
            Debug.Log("Disconnected " + description.message);
        }

        private void OnProductsFailedHandler(ProductFetchFailed failed)
        {
            Debug.Log("FAILED");
        }

        private void OnPurchasePendingHandler(PendingOrder order)
        {
            if (order == null) return;
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

            foreach (var product in order.CartOrdered.Items())
            {
                if (product.Product.definition.id.Equals(COINS_100_ID))
                {
                    OnPlayerBought100Coins?.Invoke();
                }
                else if (product.Product.definition.id.Equals(NO_ADS_ID))
                {
                    OnPlayerBoughtNoAds?.Invoke();
                }
                Debug.Log($"Order: {product.Product.definition.id}, Status: Confirmed ");
            }
        
        }

        private void OnPurchaseFailedHandler(FailedOrder failedOrder)
        {
            Debug.Log($"Purchase failed: {failedOrder.FailureReason}, Details: {failedOrder.Details}");
        }


    }
}