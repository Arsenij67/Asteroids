using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Asteroid.Database.Connection
{
    public class Connector : IDisposable
    {
        protected event Action OnInternetConnected;
        protected bool IsConnected;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private IInstanceLoader _instanceLoader;

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }

        protected void Initialize(IInstanceLoader instanceLoader)
        {
            _instanceLoader = instanceLoader;
            _cancellationTokenSource = _instanceLoader.CreateInstance<CancellationTokenSource>();
            _cancellationToken = _instanceLoader.CreateInstance<CancellationToken>();
            WaitForConnection();
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
                            Debug.Log($"Интернет подключен (проверка через {address})");
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

            while (!_cancellationToken.IsCancellationRequested)
            {
                if (await IsConnectionAvailable())
                {
                    OnInternetConnected();
                }

                await UniTask.Delay(1000, cancellationToken: _cancellationToken);
            }
        }
    }
}