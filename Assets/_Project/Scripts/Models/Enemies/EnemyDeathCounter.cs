using Asteroid.Statistic;
using Asteroid.Database;
using UnityEngine;
using Asteroid.Generation;
using UnityEditor.Playables;
using Asteroid.Enemies;

public class EnemyDeathCounter
{
    private ShipStatisticsModel _shipStatisticModel;
    public void Initialize(ShipStatisticsModel shipStatisticModel)
    {
        _shipStatisticModel = shipStatisticModel;
    }

    public void OnEnemyDied(BaseEnemy enemy)
    {
        IncreaseKilledEnemies(enemy);
    }

    private void IncreaseKilledEnemies(BaseEnemy enemy)
    {
        enemy.AddToStatistic();
    }

 
}
