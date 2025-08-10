using Asteroid.Generation;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.Services.UnityAdvertisement;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Asteroid.Installers
{
    public class ResourcesInitializerInstaller : BaseResourcesInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.BindInterfacesAndSelfTo<BootstrapController>().AsSingle();
            Container.Bind<List<UniTask>>().AsTransient();
        }
    }
}
