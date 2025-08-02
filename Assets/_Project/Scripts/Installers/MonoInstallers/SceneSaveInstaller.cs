using Asteroid.Generation;
using Zenject;
using Asteroid.Generation;

namespace Asteroid.Installers.MonInstallers
{
    public class SceneSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalBundleSceneLoader>().AsSingle();
            Container.Bind<BootstrapSceneData>().FromScriptableObjectResource("ScriptableObjects/BootstrapSceneData").AsSingle();
        }
    }
}