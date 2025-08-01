using Asteroid.Generation;
using Asteroid.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

public class ResourcesInitializerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
        Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<BootstrapController>().AsSingle();
        Container.Bind<List<UniTask>>().AsTransient();
    }
}
