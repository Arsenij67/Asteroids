using Asteroid.Database;
using Asteroid.Generation;
using Asteroid.Services;
using Asteroid.Services.Analytics;
using Asteroid.Services.IAP;
using Asteroid.Services.UnityAdvertisement;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

namespace Asteroid.Installers
{
    public class BaseResourcesInstaller : MonoInstaller<BaseResourcesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UnityAdsAdvertisement>().AsSingle();
            Container.Bind<List<UniTask>>().AsTransient();
            Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
            Container.Bind<AdvertisementController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
            Container.Bind<DataSave>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<IAPAnalyzer>().FromNew().AsSingle();
        }
    }
}
