using Asteroid.Database;
using Asteroid.Database.Connection;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService : IDisposable
    {
        public event Func<int, UniTask> OnPlayerBought100Coins;
        public event Func<bool, UniTask> OnPlayerBoughtNoAds;
        public UniTask Initialize();
        public void BuyNoAds();
        public void Buy100Coins();

        public bool IsInitialized { get; }

    }
}
