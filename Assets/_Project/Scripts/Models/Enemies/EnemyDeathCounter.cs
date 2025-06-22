using Asteroid.Statistic;
using Asteroid.Database;
using UnityEngine;
using Asteroid.Generation;
using UnityEditor.Playables;

public class EnemyDeathCounter
{
    private ShipStatisticsModel _shipStatisticModel;
    public void Initialize(ShipStatisticsModel shipStatisticModel)
    {
        _shipStatisticModel = shipStatisticModel;
    }

    public void OnEnemyDied()
    {
        IncreaseKilledEnemies();
    }

    private void IncreaseKilledEnemies()
    {
        _shipStatisticModel.EnemiesDestroyed++;
    }

 
}
