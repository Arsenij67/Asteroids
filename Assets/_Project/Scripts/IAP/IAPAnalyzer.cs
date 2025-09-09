using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Asteroid.Services
{
    public class IAPAnalyzer
    {
        [SerializeField] private const string NO_ADS_ID= "NO ADS";

        private StoreController _storeController;
        public async UniTask Initialize()
        {
            _storeController = UnityIAPServices.StoreController();

            _storeController.OnPurchasePending += OnPurchasePending;

            await _storeController.Connect();

            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnProductsFetchFailed += OnProductsFailed;
            _storeController.OnPurchasesFetched += OnPurchasesFetched;

            var initialProductsToFetch = new List<ProductDefinition>
            {
                new(NO_ADS_ID, ProductType.NonConsumable)
            };

            _storeController.FetchProducts(initialProductsToFetch);
        }

        private void OnProductsFailed(ProductFetchFailed failed)
        {
            Debug.Log("FAILED");
        }

        void OnPurchasePending(PendingOrder order)
        {
            Debug.Log("PENDING");
        }

        void OnProductsFetched(List<Product> products)
        {
            _storeController.FetchPurchases();
        }
        void OnPurchasesFetched(Orders orders)
        {
            Debug.Log("FETCHED");
            _storeController.PurchaseProduct(NO_ADS_ID);
            // Process purchases, e.g. check for entitlements from completed orders 
        }
    }
}