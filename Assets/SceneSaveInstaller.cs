using Asteroid.Generation;
using UnityEngine;
using Zenject;

public class SceneSaveInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var instanceLocalBundle = Container.Instantiate<LocalBundleSceneLoader>();
        var instanceCreator = Container.Instantiate<InstanceCreator>();
        Container.Bind<ISceneLoader>().FromInstance(instanceLocalBundle).AsSingle().NonLazy();
        Container.Bind<IInstanceLoader>().FromInstance(instanceCreator).AsSingle().NonLazy();
        Debug.Log(name);
    }
}