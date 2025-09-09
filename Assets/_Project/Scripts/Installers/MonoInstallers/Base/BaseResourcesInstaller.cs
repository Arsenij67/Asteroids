using Asteroid.Generation;
using Asteroid.Services;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Zenject;

namespace Asteroid.Installers
{
    public class BaseResourcesInstaller : MonoInstaller<BaseResourcesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
            Container.Bind<AdvertisementController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsAdvertisement>().AsSingle();
            Container.BindInterfacesAndSelfTo<IAPAnalyzer>().FromNew().AsSingle();
        }
    }
}
