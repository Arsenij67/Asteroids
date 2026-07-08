using Asteroid.Generation;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
using Asteroid.Services.RemoteConfig;
using Asteroid.SpaceShip;
using Asteroid.Statistic;
using Zenject;

namespace Asteroid.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<AnalyticsEventHandler>().FromNew().AsSingle();
            Container.Bind<EntitiesGenerationPresenter>().FromNew().AsSingle();
            Container.Bind<ShipStatisticsModel>().FromNew().AsSingle();
            Container.Bind<EnemyDeathCounter>().FromNew().AsSingle();
            Container.Bind<ShipStatisticsPresenter>().FromNew().AsSingle();
            Container.Bind<EntitiesGenerationData>().FromMethod((context) => context.Container.Resolve<BaseResourceLoaderService>().LoadResource<EntitiesGenerationData>("ScriptableObjects/EntitiesGenerationData")).AsSingle();
            Container.Bind<SpaceShipData>().FromMethod((context) => context.Container.Resolve<BaseResourceLoaderService>().LoadResource<SpaceShipData>("ScriptableObjects/SpaceShipData")).AsSingle();
        }
    }
    
}
