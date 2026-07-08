using Asteroid.Generation;
using Asteroid.Services.RemoteConfig;
using UnityEngine;
using Zenject;

namespace Asteroid.Installers.MonInstallers
{
    public class SceneSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BaseResourceLoaderService>().To<BaseResourceLoaderService>().AsTransient();
            Container.BindInterfacesAndSelfTo<LocalBundleSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigService>().FromNew().AsSingle().NonLazy();
            Container.Bind<BootstrapSceneData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<BootstrapSceneData>("ScriptableObjects/BootstrapSceneData")).AsSingle();
            Container.Bind<ShopSceneData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<ShopSceneData>("ScriptableObjects/ShopSceneData")).AsSingle();
        }
    }
}