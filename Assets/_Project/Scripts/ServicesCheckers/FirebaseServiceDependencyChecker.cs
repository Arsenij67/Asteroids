using Cysharp.Threading.Tasks;
using UnityEngine;
using FirebaseWebGL;

public class FirebaseServiceDependencyChecker : IServiceDependencyChecker
{
    public bool IsAvailable => _application.isInitialized;

    public string ErrorStatus => !_application.isInitialized ? "Init bad" : "Initialization firebase successful!";

    private FirebaseApp _application;

    public UniTask CheckAndFixDependenciesAsync()
    {
        _application = FirebaseWebGL.FirebaseApp.DefaultInstance();

        if (_application.isInitialized)
        {  
            return UniTask.CompletedTask;
        }
        return UniTask.WhenAll(CheckAnalytics(_application), CheckRemoteConfig(_application));
    }

    private UniTask CheckAnalytics(FirebaseApp application) 
    {
        if (application.Analytics != null)
        {
            bool initialized = false;
            application.Analytics.Initialize(callbackStatus =>
            {
                if (!callbackStatus.success)
                {
                    Debug.LogError($"Initialize: {callbackStatus.error}");
                    return;
                }

                initialized = callbackStatus.result;
            });
            if (!initialized)
            {
                Debug.LogError("Initialize: not initialized");
                return UniTask.CompletedTask;
            }

        }
        return UniTask.CompletedTask;

    }

    private UniTask CheckRemoteConfig(FirebaseApp application)
    {
        application.RemoteConfig.Activate((callbackStatus) =>
        {
            if (!callbackStatus.success)
            {
                Debug.LogError($"Activate: {callbackStatus.error}");
                return;
            }
            Debug.Log($"Activate: {callbackStatus.result}");
        });
        return UniTask.CompletedTask;
    }



}
