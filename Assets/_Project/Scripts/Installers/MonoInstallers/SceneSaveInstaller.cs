using Asteroid.Generation;
using Asteroid.Services.RemoteConfig;
using Zenject;

namespace Asteroid.Installers.MonInstallers
{
    public class SceneSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalBundleSceneLoader>().FromMethod((context)=>context.Container.Resolve<InstanceCreator>().CreateInstance<LocalBundleSceneLoader>()).AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigService>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<FirebaseRemoteConfigService>()).AsSingle();
            Container.Bind<BootstrapSceneData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<BootstrapSceneData>("ScriptableObjects/BootstrapSceneData")).AsSingle();
            Container.Bind<ShopSceneData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<ShopSceneData>("ScriptableObjects/ShopSceneData")).AsSingle();
        }
    }
}