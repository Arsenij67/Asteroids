using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database.Connection
{
    public class Connector
    {
        protected bool IsConnected;   

        protected async UniTask<bool> IsConnectionAvailable()
        {
            string[] _dnsAddresses = new string[] { "https://yandex.ru", "1.1.1.1", "www.microsoft.com" };
            bool isConnected = false;
            const int TIME_WAIT_CALLBACK = 2;
            foreach (string address in _dnsAddresses)
            {
                try
                {
                    using (UnityWebRequest request = UnityWebRequest.Head(address))
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
                    Debug.Log("нет подключения к интернету");
                }
            }
            IsConnected = isConnected;
            return isConnected;

        }
    }
}
