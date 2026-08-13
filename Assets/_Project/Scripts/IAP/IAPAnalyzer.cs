using Asteroid.Database;
using Asteroid.Database.Connection;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;


namespace Asteroid.Services.IAP
{
    public class IAPAnalyzer : IPurchasingService
    {
        private const string NO_ADS_ID = "NO ADS";
        private const string COINS_100_ID = "COINS 100";

        public event Func<int, UniTask> OnPlayerBought100Coins;
        public event Func<bool, UniTask> OnPlayerBoughtNoAds;

        public bool IsInitialized { get; private set; }

        private readonly int _added100Coins = 100;
        private readonly bool _advertisementIsCanceled = true;
        private readonly StoreController _storeController = UnityIAPServices.StoreController();

        public async UniTask Initialize()
        {
            if (IsInitialized) return;
            await _storeController.Connect();
            SubscribeListeners();
            var productCatalog = ProductCatalog.LoadDefaultCatalog();
            var initialProductsToFetch = productCatalog.allProducts.Select(item => new ProductDefinition(item.id, item.type)).ToList();
            _storeController.FetchProducts(initialProductsToFetch);
            IsInitialized = true;
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
            UnsubscribeListeners();
            IsInitialized = false;
        }

        private async void BuyProduct(string productId)
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
        }

        private void OnPurchasesConfirmedHandler(Order order)
        {
            var items = order.CartOrdered.Items();
            foreach (var product in order.CartOrdered.Items())
            {
                if (product.Product.definition.id.Equals(COINS_100_ID))
                {
                   OnPlayerBought100Coins?.Invoke(_added100Coins);
                }
                else if (product.Product.definition.id.Equals(NO_ADS_ID))
                {
                    OnPlayerBoughtNoAds?.Invoke(_advertisementIsCanceled);
                }
                Debug.Log($"Order: {product.Product.definition.id}, Status: Confirmed ");
            }
        }

        private void OnPurchaseFailedHandler(FailedOrder failedOrder)
        {
            Debug.Log($"Purchase failed: {failedOrder.FailureReason}, Details: {failedOrder.Details}");
        }

        private void SubscribeListeners()
        {
            _storeController.OnPurchasePending += OnPurchasePendingHandler;
            _storeController.OnProductsFetched += OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed += OnProductsFailedHandler;
            _storeController.OnStoreDisconnected += OnStoreDisconnectedHandler;
            _storeController.OnPurchaseFailed += OnPurchaseFailedHandler;
            _storeController.OnPurchaseConfirmed += OnPurchasesConfirmedHandler;
        }

        private void UnsubscribeListeners()
        {
            _storeController.OnPurchasePending -= OnPurchasePendingHandler;
            _storeController.OnProductsFetched -= OnProductsFetchedHandler;
            _storeController.OnProductsFetchFailed -= OnProductsFailedHandler;
            _storeController.OnStoreDisconnected -= OnStoreDisconnectedHandler;
            _storeController.OnPurchaseFailed -= OnPurchaseFailedHandler;
            _storeController.OnPurchaseConfirmed -= OnPurchasesConfirmedHandler;
        }
    }
}