using UnityEngine;

namespace Asteroid.Inputs
{
    public interface IDeviceInput
    {
        public float ScanMove();
        public float ScanRotation();

    }
}
