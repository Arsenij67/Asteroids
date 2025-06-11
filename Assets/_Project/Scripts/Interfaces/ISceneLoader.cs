
using Cysharp.Threading.Tasks;

namespace Asteroid.Generation
{
    public interface ISceneLoader
    {
        public void LoadScene(string name);
        public UniTask LoadSceneAsync(string name);
        public void ReloadScene();
        
    }
}
