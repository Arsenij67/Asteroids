using Cysharp.Threading.Tasks;

public interface IServiceDependencyChecker
{
    public bool IsAvailable { get; }
    public string ErrorStatus { get; }

    public UniTask CheckAndFixDependenciesAsync();
}
