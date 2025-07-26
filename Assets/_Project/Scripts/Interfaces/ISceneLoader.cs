
using Cysharp.Threading.Tasks;

namespace Asteroid.Generation
{
    public interface ISceneLoader
    {
        public float LoadingProgress { get; }
        public string LastLoadedScene { get; }
        public void LoadScene(string name);
        public UniTask<object> ReloadSceneAsync(string name);
        public void LoadSceneAdditive(string name);
        public UniTask<object> LoadSceneAsync(string name);
        public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true);
        public void ReloadScene();
        public UniTask SwitchSceneActivation(bool allowSceneBeActive);
        public void UnloadScene();
        public UniTask <object> UnloadSceneAsync(object data);

    }
}
