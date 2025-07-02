using Asteroid.Generation;
using UnityEngine;
using Zenject;

public class SceneSaveInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
       var instance = Container.Instantiate<LocalBundleSceneLoader>();
        Container.Bind<ISceneLoader>().FromInstance(instance).AsSingle();
        Debug.Log("созданы");
    }
}
