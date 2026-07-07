using Asteroid.Generation;
using Cysharp.Threading.Tasks;

namespace Asteroid.Database
{
    public interface ISaveStrategy
    {
        public void Initialize(DataSave dataSave, ShopUI shopUI = null, IRemoteSavable remoteSavable = null);
        public UniTask AddCountDeadEnemies(int enemiesToAdd);
        public UniTask AddCountCoins(int coinsToAdd);
        public void UpdateNoAdsStatus(bool adevertisementIsCanceled);
        public UniTask RemoveCountCoins(int coinsToRemove);
        public SaveChoice GetMode();
        public void UpdateLastSaveTime(string key);
    }
}