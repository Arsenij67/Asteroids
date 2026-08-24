using UnityEngine;

namespace Asteroid.SpaceObjectActions
{
    public class SpaceObject : MonoBehaviour
    {
        [field: SerializeField] public Transform Vertices { get; private set; }
        private Transform DownLeftBorder => Vertices.GetChild(3);
        private Transform UpRightBorder => Vertices.GetChild(1);

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
