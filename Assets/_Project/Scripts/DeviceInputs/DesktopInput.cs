using UnityEngine;

namespace Asteroid.Inputs
{
    public class DesktopInput : IDeviceInput
    {
        public float ScanMove()
        {
            float positionInput = Input.GetAxis("Vertical");
            return  positionInput;
        }

        public float ScanRotation()
        {
            float rotationInput = Input.GetAxis("Horizontal");
            return rotationInput;
        }
    }
}
