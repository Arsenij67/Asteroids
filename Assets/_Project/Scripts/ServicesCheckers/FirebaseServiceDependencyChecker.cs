using Cysharp.Threading.Tasks;
using Firebase;

public class FirebaseServiceDependencyChecker : IServiceDependencyChecker
{
    private DependencyStatus _status;

    public bool IsAvailable => _status == DependencyStatus.Available;

    public string ErrorStatus => _status != DependencyStatus.Available ? _status.ToString() : "Initialization firebase successful!";

    public async UniTask CheckAndFixDependenciesAsync()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
       _status = dependencyStatus;
    }
}
