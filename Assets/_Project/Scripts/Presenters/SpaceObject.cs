using UnityEngine;

namespace Asteroid.SpaceObjectActions
{
    public class SpaceObject : MonoBehaviour
    {
        [field: SerializeField] public Vector2 DownLeftBorder { get; private set; }
        [field: SerializeField] public Vector2 UpRightBorder { get; private set; }

        public bool TryTeleport(Vector2 position)
        {
            Vector2 newPosition = position;

            if (position.x < DownLeftBorder.x)
            {
                newPosition = new Vector2(UpRightBorder.x, position.y);
            }
            else if (position.x > UpRightBorder.x)
            {
                newPosition = new Vector2(DownLeftBorder.x, position.y);
            }
            else if (position.y < DownLeftBorder.y)
            {
                newPosition = new Vector2(position.x, UpRightBorder.y);
            }
            else if (position.y > UpRightBorder.y)
            {
                newPosition = new Vector2(position.x, DownLeftBorder.y);
            }
            transform.localPosition = newPosition;

            return !(newPosition == position);
        }
    }
}
