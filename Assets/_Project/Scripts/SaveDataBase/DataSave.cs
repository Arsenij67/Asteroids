using System;

namespace Asteroid.Database
{
    [Serializable]
    public struct DataSave
    {
        public string Name;
        public int EnemiesDestroyed;
        public bool IsLaserUsed;

    }
}
