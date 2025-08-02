using Asteroid.Enemies;
using System;
using UnityEngine;

namespace Asteroid.Statistic
{
    public struct ShipStatisticsModel
    {
        public int CountShootsFireball;
        public int CountShootsLaser;
        public int CountDestroyedUFO;
        public int CountDestroyedAsteroids;
        public int CountDestroyedMeteorites;

        public int CountShoots => CountShootsFireball + CountShootsLaser;
        public int EnemiesDestroyed => CountDestroyedUFO + CountDestroyedAsteroids + CountDestroyedMeteorites;
        public bool LaserWasUsed => Convert.ToBoolean(CountShootsLaser);

    }

}