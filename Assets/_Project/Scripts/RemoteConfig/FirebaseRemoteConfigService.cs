using Firebase.Extensions;
using Firebase.RemoteConfig;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services.RemoteConfig
{ 
public class FirebaseRemoteConfigService : IRemoteConfigService
{
        public bool IsInitialized => throw new NotImplementedException();

        public event Action OnConfigUpdated;

        // Invoke the listener.
        private async  void  Start()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
  new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("test_string", "new value");
        var rc = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task => { });

        await FetchDataAsync();

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
          += ConfigUpdateListenerEventHandler;
        Debug.Log("Start");
    }

    // Handle real-time Remote Config events.
   private void ConfigUpdateListenerEventHandler(

       object sender, Firebase.RemoteConfig.ConfigUpdateEventArgs args)
    {
        Debug.Log("Обновления замечены");

        if (args.Error != Firebase.RemoteConfig.RemoteConfigError.None)
        {
            Debug.Log(String.Format("Error occurred while listening: {0}", args.Error));
            return;
        }

        Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));
        // Activate all fetched values and then display a welcome message.
        FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
          task => {
              DisplayWelcomeMessage();
          });
    }

    private void DisplayWelcomeMessage()
    {
        Debug.Log("Обновления замечены");
    }

    // Stop the listener.
    void OnDestroy()
    {
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
          -= ConfigUpdateListenerEventHandler;
    }
    private async void Awake()
    {


 
    }

 
    public UniTask FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete).AsUniTask(); 
    }
    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
               ConfigValue configVal =  remoteConfig.GetValue("test_string");
               ConfigValue five =  remoteConfig.GetValue("five");
                Debug.Log("ValueRemoteConfig: "+configVal.StringValue);
                Debug.Log("five: "+five.LongValue);
            });
    }

        public UniTask Initialize()
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public UniTask FetchAndActivateAsync()
        {
            throw new NotImplementedException();
        }
    }

