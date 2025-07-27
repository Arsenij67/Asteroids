using Asteroid.Generation;
using Zenject;

namespace Asteroid.Installers.MonInstallers
{
    public class SceneSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalBundleSceneLoader>().AsSingle();
        }
    }
}