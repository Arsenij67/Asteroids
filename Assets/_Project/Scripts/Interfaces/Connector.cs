using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database.Connection
{
    public class Connector
    {
        protected event Func<UniTask> OnInternetConnected;
        protected event Func<UniTask> OnInternetDisconnected;

        protected IInstanceLoader _instanceLoader;
        protected bool IsConnected;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        protected void Initialize(IInstanceLoader instanceLoader)
        {
            _instanceLoader = instanceLoader;
            _cancellationTokenSource = _instanceLoader.CreateInstance<CancellationTokenSource>();
        }

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
                            break;
                        }
                    }
                }
                catch (UnityWebRequestException ex)
                {
                    Debug.Log($"Нет подключения к интернету через {address}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка проверки интернета через {address}: {ex.Message}");
                }
            }
            IsConnected = isConnected;
            return isConnected;
        }

        protected async UniTask WaitForConnection()
        {
            const int TIME_WAIT_CALLBACK = 1*1000;
            _cancellationToken = _instanceLoader.CreateInstance<CancellationToken>();
            while (!_cancellationToken.IsCancellationRequested)
            {
                Debug.Log("Ждем подключения");
                if (await IsConnectionAvailable())
                {
                    OnInternetConnected.Invoke();
                    Debug.Log("Интерент дали!!!!");
                    break;
                }
              
                await UniTask.Delay(TIME_WAIT_CALLBACK, cancellationToken:_cancellationToken);
            }
        }

        protected async UniTask WaitForDisconnection()
        {
            const int TIME_WAIT_CALLBACK = 1 * 1000;
            _cancellationToken = _instanceLoader.CreateInstance<CancellationToken>();
            while (!_cancellationToken.IsCancellationRequested)
            {
                Debug.Log("Ждем отключения");
                if (!await IsConnectionAvailable())
                {
                    OnInternetDisconnected.Invoke();
                    Debug.Log("Интернет забрали!");
                   break;
                }

                await UniTask.Delay(TIME_WAIT_CALLBACK, cancellationToken: _cancellationToken);
            }
        }
    }
}