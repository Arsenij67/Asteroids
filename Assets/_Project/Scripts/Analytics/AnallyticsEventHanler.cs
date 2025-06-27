using Asteroid.Generation;
using Asteroid.Statistic;
using Asteroid.Weapon;
using Firebase.Analytics;
using System;
using UnityEngine;

namespace Asteroid.Services
{
    public class AnalyticsEventHandler
    {
        private IInstanceLoader _instanceLoader;
        private IAnalytics _analytics;
        private ShipStatisticsModel _shipStatisticModel;
        private SpaceEntryPoint _spaceEntryPoint;
        private LaserWeaponController _laserWeapon;
        public void Initialize(SpaceEntryPoint spaceEntryPoint,ShipStatisticsModel shipStatisticsModel, LaserWeaponController laserWeapon)
        {
            _instanceLoader = new InstanceCreator();
            _analytics = _instanceLoader.CreateInstance<Asteroid.Services.FirebaseAnalyticsSender>();
            _spaceEntryPoint = spaceEntryPoint;
            _shipStatisticModel = shipStatisticsModel;
            _laserWeapon = laserWeapon;
            _spaceEntryPoint.OnGameStarted += SendEventGameStart;
            _spaceEntryPoint.OnPlayerDied += SendEventGameEnd;
            _laserWeapon.OnLaserTurned += SendEventLaserUsed;

        }
        public void SendEventGameStart()
        {
            CheckActivation();
            _analytics.PushEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, Firebase.Analytics.FirebaseAnalytics.ParameterStartDate, DateTime.UtcNow);
            Debug.Log($"Отправлено уведомление о начале игры: {Firebase.Analytics.FirebaseAnalytics.EventLogin} {Firebase.Analytics.FirebaseAnalytics.ParameterStartDate} {DateTime.UtcNow}");
        }

        public void SendEventGameEnd()
        {
            CheckActivation();
            Parameter[] parametersValue =
{
            new Parameter("count_shoots", _shipStatisticModel.CountShoots),
            new Parameter("count_laser_shoots", _shipStatisticModel.CountShootsLaser.ToString()),
            new Parameter("enemies_destroyed", _shipStatisticModel.EnemiesDestroyed)
            };
            _analytics.PushEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd, Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, parametersValue);
            Debug.Log($"Отправлено уведомление о конце игры: {_shipStatisticModel.CountShoots} {_shipStatisticModel.CountShootsLaser.ToString()} {_shipStatisticModel.EnemiesDestroyed}");
        }

        public void SendEventLaserUsed()
        {
            CheckActivation();
            _analytics.PushEvent(Firebase.Analytics.FirebaseAnalytics.EventSelectItem, "laser_used_now", DateTime.UtcNow);
            Debug.Log($"Отправлено уведомление об использовании лазера: !");
        }

        private void CheckActivation()
        {
            if (!_analytics.AnalyticsEnabled) return;
        }

    }
}
