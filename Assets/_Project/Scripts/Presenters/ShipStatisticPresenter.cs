namespace Asteroid.Statistic
{
    public class ShipStatisticPresenter
    {
        private ShipStatisticsModel _shipStatisticModel;
        private ShipStatisticsView _shipStatisticView;

        public void Initialize(ShipStatisticsModel shipStatisticModel, ShipStatisticsView shipStatisticView)
        {
            _shipStatisticModel = shipStatisticModel;
            _shipStatisticView = shipStatisticView;
        }

        public void UpdateCountLaserShoots(int countBullets)
        {
            _shipStatisticModel.CountShootsLaser = countBullets;
            _shipStatisticView.UpdateLaserCount(_shipStatisticModel.CountShootsLaser);
        }

        public void UpdateRollbackTime(float attackTime)
        {
            _shipStatisticModel.LaserRollbackTime = attackTime;
            _shipStatisticView.UpdateRollbackTime(_shipStatisticModel.LaserRollbackTime);
        }

        public void UpdateCountBulletShoots(int countBullets)
        {
            _shipStatisticModel.CountShootsFireball = countBullets;
            _shipStatisticView.UpdateFireballCount(_shipStatisticModel.CountShootsFireball);
        }

        public void IncreaseCountUFODestroyed()
        {
            _shipStatisticModel.CountDestroyedUFO++;
        }

        public void IncreaseCountMeteoritesDestroyed()
        {
            _shipStatisticModel.CountDestroyedMeteorites++;
        }

        public void IncreaseCountAsteroidsDestroyed()
        {
            _shipStatisticModel.CountDestroyedAsteroids++;
        }

    }
}
