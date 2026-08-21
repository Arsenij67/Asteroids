using UnityEngine;

namespace Asteroid.Inputs
{
    public interface IDeviceInput
    {
        public void Initialize <T> (T InputSource);
        public float ScanMove();
        public float ScanRotation();

    }
}
