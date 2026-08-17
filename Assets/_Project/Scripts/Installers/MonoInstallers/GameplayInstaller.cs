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
            Container.Bind<AnalyticsEventHandler>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<AnalyticsEventHandler>()).AsTransient();
            Container.Bind<EntitiesGenerationFactory>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<EntitiesGenerationFactory>()).AsTransient();
            Container.Bind<ShipStatisticsModel>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<ShipStatisticsModel>()).AsTransient();
            Container.Bind<ShipStatisticPresenter>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<ShipStatisticPresenter>()).AsTransient();
            Container.Bind<EnemyDeathCounter>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<EnemyDeathCounter>()).AsTransient();
            Container.Bind<GameOverPresenter>().FromMethod((context) => context.Container.Resolve<InstanceCreator>().CreateInstance<GameOverPresenter>()).AsSingle();
            Container.Bind<EntitiesGenerationData>().FromMethod((context) => context.Container.Resolve<AddressableBundleLoader>().LoadResource<EntitiesGenerationData>("Assets/_Project/Scripts/Models/Data/EntitiesGenerationData")).AsTransient();
            Container.Bind<SpaceShipData>().FromMethod((context) => context.Container.Resolve<BaseResourceLoaderService>().LoadResource<SpaceShipData>("ScriptableObjects/SpaceShipData")).AsTransient();
        }
    }
    
}
