using Asteroid.Enemies;

namespace Asteroid.Statistic
{
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
}
