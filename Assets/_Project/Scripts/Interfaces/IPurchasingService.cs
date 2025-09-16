using Asteroid.Database;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services.IAP
{
    public interface IPurchasingService
    {
        public UniTask Initialize(BootstrapUI bootstrapUI, DataSave dataSave);
        public void BuyNoAds();
        public void Buy100Coins();

    }
}
