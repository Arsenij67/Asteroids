using Firebase.Extensions;
using Firebase.RemoteConfig;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseRemoteConfigService : MonoBehaviour
{
    // Invoke the listener.
    void Start()
    {
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
          += ConfigUpdateListenerEventHandler;
    }

    // Handle real-time Remote Config events.
   private void ConfigUpdateListenerEventHandler(
       object sender, Firebase.RemoteConfig.ConfigUpdateEventArgs args)
    {
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


        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("config_test_string", "new value");
        //defaults.Add("config_test_int", 1);
        //defaults.Add("config_test_float", 1.0);
        //defaults.Add("config_test_bool", false);

        var rc = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task => { });

       await FetchDataAsync();
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
               ConfigValue configVal =  remoteConfig.GetValue("config_test_string");
                Debug.Log("ValueRemoteConfig: "+configVal.StringValue);
            });
    }
}

