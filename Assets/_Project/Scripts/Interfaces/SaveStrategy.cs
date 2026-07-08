using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database
{
    public abstract class SaveStrategy
    {
        public bool NoAdsStatus => (bool)(DataSave[KeyData.ADS_DISABLED] ?? false);
        public int CountCoins => (int)(DataSave[KeyData.COINS_COUNT] ?? 0);

        protected ShopUI ShopUI;
        protected DataSave DataSave;

        public virtual void Initialize(DataSave dataSave, ShopUI shopUI = null,IRemoteSavable remoteSavable=null)
        {
            ShopUI = shopUI;
            DataSave = dataSave;
        }
        public abstract UniTask AddCountDeadEnemies(int enemiesToAdd);
        public abstract UniTask AddCountCoins(int coinsToAdd);
        public abstract  void UpdateNoAdsStatus(bool adevertisementIsCanceled);
        public abstract UniTask RemoveCountCoins(int coinsToRemove);
        public abstract SaveChoice GetMode();
        public abstract void UpdateLastSaveTime(string key);

        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            ShopUI.UpdateViewNoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            ShopUI.UpdateCountCoins((int)DataSave[KeyData.COINS_COUNT] + countToAdd);
        }

        public async UniTask<bool> IsAvailable()
        {
            string[] _dnsAddresses = new string[] { "https://yandex.ru", "1.1.1.1", "www.microsoft.com" };
            bool isConnected = false;
            const int TIME_WAIT_CALLBACK = 2;
            foreach (string adress in _dnsAddresses)
            {
                try
                {
                    using (UnityWebRequest request = UnityWebRequest.Head(adress))
                    {
                        request.timeout = TIME_WAIT_CALLBACK;
                        await request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            isConnected = true;
                            UnityEngine.Debug.Log("Интернет подключен (проверка через HTTP)");
                            break;
                        }

                    }
                }
                catch (UnityWebRequestException)
                {
                    Debug.LogError("нет подключения к интернету");
                }
            }
            return isConnected;
        }
    }
}
