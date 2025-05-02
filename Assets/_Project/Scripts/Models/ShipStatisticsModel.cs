using UnityEngine;

namespace Asteroid.Statistic
{
    [CreateAssetMenu(fileName = "ShipStatisticsModel", menuName = "ScriptableObjects/ShipStatisticsModel")]
    public class ShipStatisticsModel:ScriptableObject
    {
        [Header("Stats")]
        public int _enemiesDestroyed = 0;
    }
}