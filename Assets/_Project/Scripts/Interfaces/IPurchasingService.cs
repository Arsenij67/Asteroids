using Asteroid.Database;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService
    {
        public event Action OnPlayerBought100Coins;
        public event Action OnPlayerBoughtNoAds;
        public UniTask Initialize(DataSave dataSave);
        public void BuyNoAds();
        public void Buy100Coins();

    }
}
