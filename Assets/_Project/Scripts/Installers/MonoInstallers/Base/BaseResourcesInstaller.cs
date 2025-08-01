using Asteroid.Generation;
using Asteroid.Services;
using UnityEngine;
using Zenject;

public class BaseResourcesInstaller : MonoInstaller<BaseResourcesInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InstanceCreator>().AsSingle();
        Container.BindInterfacesAndSelfTo<LocalBundleLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirebaseAnalyticsSender>().AsSingle();
    }
}
