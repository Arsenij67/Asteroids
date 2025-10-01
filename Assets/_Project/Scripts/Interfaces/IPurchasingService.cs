using Asteroid.Database;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService
    {
        public UniTask Initialize(DataSave dataSave);
        public void Initialize(ShopUI dataSave);
        public void BuyNoAds();
        public void Buy100Coins();

    }
}
