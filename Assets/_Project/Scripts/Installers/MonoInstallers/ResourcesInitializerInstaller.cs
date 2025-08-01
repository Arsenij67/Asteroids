using Asteroid.Generation;
using Asteroid.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

public class ResourcesInitializerInstaller : BaseResourcesInstaller
{
    public override void InstallBindings()
    {
        base.InstallBindings();
        Container.BindInterfacesAndSelfTo<BootstrapController>().AsSingle();
        Container.Bind<List<UniTask>>().AsTransient();
    }
}
