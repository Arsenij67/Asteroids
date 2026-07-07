using Asteroid.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Services.UnityCloud
{
    public class BaseSaveDataPresenter
    {
        protected ShopUI _shopUI;
        protected DataSave _dataSave;

        public void Initialize(DataSave dataSave, ShopUI shopUI = null)
        {
            _shopUI = shopUI;
            _dataSave = dataSave;
        }
        public void UpdateUINoAds(bool isAdvertisementCanceled)
        {
            _shopUI.UpdateViewNoAds(isAdvertisementCanceled);
        }

        public void UpdateUICountCoins(int countToAdd)
        {
            _shopUI.UpdateCountCoins((int)_dataSave[KeyData.COINS_COUNT] + countToAdd);
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
