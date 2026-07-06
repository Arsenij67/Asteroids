using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Asteroid.Database
    {
    public class SavingModeDeterminer
    {
        public void Initialize()
        {
            
        }

        public async UniTask<bool> CheckInternetConnection()
        {
            string[] _dnsAdresses = new string[] { "https://yandex.ru", "1.1.1.1", "www.microsoft.com" };
            bool isConnected = false;
            const int TIME_WAIT_CALLBACK = 2;
            foreach (string adress in _dnsAdresses)
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
