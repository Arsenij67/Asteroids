using Cysharp.Threading.Tasks;
using System;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService : IDisposable
    {
        public event Func<int, UniTask> OnPlayerBought100Coins;
        public event Func<bool, UniTask> OnPlayerBoughtNoAds;

        public bool IsInitialized { get; }

        public UniTask Initialize();
        public void BuyNoAds();
        public void Buy100Coins();
    }
}
