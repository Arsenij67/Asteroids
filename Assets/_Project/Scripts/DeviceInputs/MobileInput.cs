using UnityEngine;

namespace Asteroid.Inputs
{
    public class MobileInput : IDeviceInput
    {
        private Joystick _joystick;

        public void Initialize<T>(T joystick)
        {
            _joystick = joystick as Joystick;
        }

        public float ScanMove()
        {
            return _joystick.Direction.magnitude;
        }

        public float ScanRotation()
        {
            Vector2 direction = _joystick.Direction;

            float angle = Vector2.SignedAngle(Vector2.up, direction);

            float normalizedAngle = Mathf.Clamp(angle / Mathf.Rad2Deg * Mathf.PI*2, -1f, 1f);
            return -normalizedAngle;
        }
    }
}