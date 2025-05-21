using Asteroid.Statistic;
using UnityEngine;

public class EnemyDeathTracker
{
    private ShipStatisticsModel _shipStatisticModel;
    public void Initialize(ShipStatisticsModel shipStatisticModel)
    {
        _shipStatisticModel = shipStatisticModel;
    }

    public void OnEnemyDied()
    {
        _shipStatisticModel.EnemiesDestroyed++;
    }
}
