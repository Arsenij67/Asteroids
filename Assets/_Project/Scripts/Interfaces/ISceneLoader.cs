using Cysharp.Threading.Tasks;

namespace Asteroid.Generation
{
    public interface ISceneLoader
    {
        public void LoadScene(string name);
        public UniTask ReloadSceneAsync(string name);
        public void LoadSceneAdditive(string name);
        public UniTask LoadSceneAsync(string name, bool activateOnLoad);
        public UniTask LoadSceneAdditiveAsync(string name, bool allowSceneActivate = true);
        public void ReloadCurrentScene();
        public UniTask SwitchSceneActivation(string name,bool allowSceneBeActive);
        public void UnloadScene(string name);
        public UniTask <object> UnloadSceneAsync(string name);

    }
}
