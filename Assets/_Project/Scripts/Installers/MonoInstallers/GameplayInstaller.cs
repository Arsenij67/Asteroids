using Asteroid.Generation;
using Asteroid.Inputs;
using Asteroid.Services.Analytics;
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
            Container.Bind<EntitiesGenerationController>().FromNew().AsSingle();
            Container.Bind<ShipStatisticsModel>().FromNew().AsSingle();
            Container.Bind<EnemyDeathCounter>().FromNew().AsSingle();
            Container.Bind<ShipStatisticsController>().FromNew().AsSingle();

            Container.Bind<EntitiesGenerationData>().FromNewScriptableObjectResource("ScriptableObjects/EntitiesGenerationData").AsSingle();
            Container.Bind<SpaceShipData>().FromNewScriptableObjectResource("ScriptableObjects/SpaceShipData").AsSingle();

        }
    }
}
