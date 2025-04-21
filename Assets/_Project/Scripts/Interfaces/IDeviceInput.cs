using UnityEngine;

namespace Asteroid.Inputs
{
    public interface IDeviceInput
    {
        float ScanMove();
        float ScanRotation();

    }
}
