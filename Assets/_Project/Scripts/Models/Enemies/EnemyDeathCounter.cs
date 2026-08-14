using Asteroid.Enemies;

public class EnemyDeathCounter
{
    public void OnEnemyDied(Enemy enemy)
    {
        IncreaseKilledEnemies(enemy);
    }

    private void IncreaseKilledEnemies(Enemy enemy)
    {
        enemy.AddToStatistic();
    }
 
}
