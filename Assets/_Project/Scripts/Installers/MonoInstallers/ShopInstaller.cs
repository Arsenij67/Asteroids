using Asteroid.Generation;
using Asteroid.Services.IAP;
using Asteroid.Services.RemoteConfig;
using Zenject;

namespace Asteroid.Installers.MonInstallers
{
    public class ShopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ShopSceneData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<ShopSceneData>("ScriptableObjects/ShopSceneData")).AsSingle();
            Container.Bind<PurchasesResultData>().FromMethod((context)=>context.Container.Resolve<BaseResourceLoaderService>().LoadResource<PurchasesResultData>("ScriptableObjects/PurchasesResultData")).AsSingle();
            Container.Bind<PurchasesResultPresenter>().FromMethod((context)=>context.Container.Resolve<InstanceCreator>().CreateInstance<PurchasesResultPresenter>()).AsSingle();
        }
    }
}