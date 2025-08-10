using Asteroid.Generation;
using Asteroid.Services.RemoteConfig;
using Zenject;

namespace Asteroid.Installers.MonInstallers
{
    public class SceneSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalBundleSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigService>().FromNew().AsSingle().NonLazy();
            Container.Bind<BootstrapSceneData>().FromScriptableObjectResource("ScriptableObjects/BootstrapSceneData").AsSingle();
        }
    }
}