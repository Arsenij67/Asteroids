
using Cysharp.Threading.Tasks;

namespace Asteroid.Generation
{
    public interface ISceneLoader
    {
        public float LoadingProgress { get; }
        public void LoadScene(string name);
        public void LoadSceneAdditive(string name);
        public UniTask LoadSceneAsync(string name);
        public UniTask LoadSceneAsyncAdditive(string name, bool allowSceneActivate = true);
        public void ReloadScene();
        public void SwitchSceneActivation(bool allowSceneBeActive);
        
    }
}
