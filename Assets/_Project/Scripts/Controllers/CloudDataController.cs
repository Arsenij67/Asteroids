
using Asteroid.Database;
using Cysharp.Threading.Tasks;

namespace Asteroid.Services.UnityCloud
{
    public class CloudDataController
    {
        private IRemoteSavable _remoteSavable;
        public void Initialize(IRemoteSavable remoteSavable)
        { 
            _remoteSavable = remoteSavable;   
        }
        public async void AddCountDeadEnemies(int enemiesToAdd)
        {
            int oldEnemies = await _remoteSavable.GetKey<int>(CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY);
            await _remoteSavable.SaveKey(CloudKeyData.DEAD_ENEMIES_COUNT_SUMMARY, oldEnemies+enemiesToAdd);
        }
        public async void AddCountCoins(int coinsToAdd)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(CloudKeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(CloudKeyData.COINS_COUNT, oldCoins+coinsToAdd);
        }

        public async void RemoveCountCoins(int coinsToRemove)
        {
            int oldCoins = await _remoteSavable.GetKey<int>(CloudKeyData.COINS_COUNT);
            await _remoteSavable.SaveKey(CloudKeyData.COINS_COUNT, oldCoins - coinsToRemove);
        }

    }
}
