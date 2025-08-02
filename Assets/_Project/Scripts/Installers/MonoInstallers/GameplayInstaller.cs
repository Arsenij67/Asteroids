using Asteroid.Generation;
using Asteroid.Inputs;
using Asteroid.Services;
using UnityEngine;
using Zenject;

namespace Asteroid.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<AnalyticsEventHandler>().AsSingle();
            Container.Bind<EntitiesGenerationController>().FromNew().AsSingle();
        }
    }
}
