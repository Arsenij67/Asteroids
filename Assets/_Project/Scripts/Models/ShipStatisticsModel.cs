using Asteroid.Enemies;
using System;
using UnityEngine;

namespace Asteroid.Statistic
{
    public class ShipStatisticsModel
    {
        public int CountShootsFireball = 0;
        public int CountShootsLaser = 0;
        public int CountDestroyedUFO = 0;
        public int CountDestroyedAsteroids = 0;
        public int CountDestroyedMeteorites = 0;

        public int CountShoots => CountShootsFireball + CountShootsLaser;
        public int EnemiesDestroyed => CountDestroyedUFO + CountDestroyedAsteroids + CountDestroyedMeteorites;
        public bool LaserWasUsed => Convert.ToBoolean(CountShootsLaser);

    }

}