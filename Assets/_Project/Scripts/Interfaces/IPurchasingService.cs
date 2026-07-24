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
        public event Func<int, UniTask> OnPlayerBought100Coins;
        public event Func<bool, UniTask> OnPlayerBoughtNoAds;
        public UniTask Initialize(DataSave dataSave);
        public void BuyNoAds();
        public void Buy100Coins();

    }
}
