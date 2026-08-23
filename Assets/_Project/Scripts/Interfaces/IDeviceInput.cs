using UnityEngine;

namespace Asteroid.Inputs
{
    public interface IDeviceInput
    {
        public void Initialize <T> (T InputSource);
        public Vector2 ScanMove();
        public float ScanRotation();

    }
}
