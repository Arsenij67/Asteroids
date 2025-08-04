using Asteroid.Generation;
using Asteroid.Services;
using Asteroid.UnityAdvertisement;
using Zenject;

namespace Asteroid.Installers
{
    public class BaseResourcesInstaller : MonoInstaller<BaseResourcesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsAdvertisement>().AsSingle();
            Container.Bind<AdvertisementController>().AsSingle();
        }
    }
}
