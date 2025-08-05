using Asteroid.Statistic;
using Asteroid.Database;
using UnityEngine;
using Asteroid.Generation;
using UnityEditor.Playables;
using Asteroid.Enemies;

public class EnemyDeathCounter
{
    public void OnEnemyDied(BaseEnemy enemy)
    {
        IncreaseKilledEnemies(enemy);
    }

    private void IncreaseKilledEnemies(BaseEnemy enemy)
    {
        enemy.AddToStatistic();
    }
 
}
