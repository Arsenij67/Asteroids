using Asteroid.Database;
using Asteroid.Database.Connection;
using Asteroid.Exit;
using Asteroid.Generation;
using Asteroid.Services.Analytics;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityAdvertisement;
using Asteroid.Services.UnityCloud;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

namespace Asteroid.Installers
{
    public class BaseResourcesInstaller : MonoInstaller<BaseResourcesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BaseResourceLoaderService>().AsCached();
            Container.Bind<IApplicationQuitter>().To<PCApplicationQuitter>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsAdvertisement>().AsSingle();
            Container.Bind<List<UniTask>>().AsTransient();
            Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
            Container.Bind<AdvertisementPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
            Container.Bind<DataSave>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<IAPAnalyzer>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitySaveCloud>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<KeyData>().FromNew().AsCached();
            Container.Bind<CloudDataPresenter>().FromNew().AsTransient();
            Container.Bind<LocalSaveStrategy>().To<LocalSaveStrategy>().AsCached();
            Container.Bind<LocalSaveMetaData>().FromMethod((context) => context.Container.Resolve<BaseResourceLoaderService>().LoadResource<LocalSaveMetaData>("ScriptableObjects/LocalSaveMetaData")).AsSingle();
            Container.Bind<SaveDataStrategyManager>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<SaveDataStrategyManager>()).AsSingle();
            Container.Bind<WIFIConnector>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<WIFIConnector>()).AsSingle();
        }
    }
}
