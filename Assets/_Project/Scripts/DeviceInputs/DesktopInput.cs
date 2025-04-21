using UnityEngine;

namespace Asteroid.Inputs
{
    public class DesktopInput : MonoBehaviour, IDeviceInput
    {
        public float ScanMove()
        {
            float thrustInput = Input.GetAxis("Vertical");
            return  thrustInput;
        }

        public float ScanRotation()
        {
            float rotationInput = Input.GetAxis("Horizontal");
            return rotationInput;
        }
    }
}
