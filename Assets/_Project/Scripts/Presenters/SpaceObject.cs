using UnityEngine;

namespace Asteroid.SpaceObjectActions
{
    public class SpaceObject : MonoBehaviour
    {
        private const short INDEX_CHILD_DOWN_LEFT = 3;
        private const short INDEX_CHILD_UP_RIGHT = 1;

        [field: SerializeField] public Transform Vertices { get; private set; }
        private Transform DownLeftBorder => Vertices.GetChild(INDEX_CHILD_DOWN_LEFT);
        private Transform UpRightBorder => Vertices.GetChild(INDEX_CHILD_UP_RIGHT);

        public bool TryTeleport(Vector2 position)
        {
            Vector2 newPosition = position;

            if (position.x < DownLeftBorder.position.x)
            {
                newPosition = new Vector2(UpRightBorder.position.x, position.y);
            }
            else if (position.x > UpRightBorder.position.x)
            {
                newPosition = new Vector2(DownLeftBorder.position.x, position.y);
            }
            else if (position.y < DownLeftBorder.position.y)
            {
                newPosition = new Vector2(position.x, UpRightBorder.position.y);
            }
            else if (position.y > UpRightBorder.position.y)
            {
                newPosition = new Vector2(position.x, DownLeftBorder.position.y);
            }
            transform.localPosition = newPosition;

            return !(newPosition == position);
        }
    }
}
