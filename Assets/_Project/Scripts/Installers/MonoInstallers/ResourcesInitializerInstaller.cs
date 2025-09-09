using Asteroid.Generation;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Asteroid.Services;

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
