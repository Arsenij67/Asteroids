using UnityEngine;

namespace Asteroid.Inputs
{
    public class MobileInput : IDeviceInput
    {
        private const double MIN_SESITIVITY_LENGTH = 0.1;

        private Joystick _joystick;

        public void Initialize<T>(T joystick)
        {
            _joystick = joystick as Joystick;
        }

        public Vector2 ScanMove()
        {
            if (_joystick == null)
                return Vector2.zero;

            Vector2 direction = _joystick.Direction;

            return direction.normalized;
        }

        public float ScanRotation()
        {
            if (_joystick == null)
                return 0f;

                Vector2 direction = _joystick.Direction;
                if (direction.magnitude >= MIN_SESITIVITY_LENGTH)
                {
                    float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
                return angle;
                }
                return 0f;
        }
    }
}