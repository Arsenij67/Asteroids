using Asteroid.Database;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService
    {
        public event UnityAction<int> OnPlayerBought100Coins;
        public event UnityAction<bool> OnPlayerBoughtNoAds;
        public UniTask Initialize(DataSave dataSave);
        public void BuyNoAds();
        public void Buy100Coins();

    }
}
