using UnityEngine;
using Asteroid.Statistic;
using Asteroid.Enemies;
using Asteroid.Weapon;
using Asteroid.Inputs;

namespace Asteroid.SpaceShip
{
    [RequireComponent(typeof(SpaceShipData))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IDeviceInput))]
    public class SpaceShipController : MonoBehaviour
    {
        private IDeviceInput _deviceInput;

        [SerializeField] private ShipStatisticsView _statisticsView;
        [SerializeField] private ShipStatisticsController _statisticsController;

        private SpaceShipData _shipData;
        private Rigidbody2D _rbController;
        private void Awake()
        {
            _shipData = GetComponent<SpaceShipData>();
            _rbController = GetComponent<Rigidbody2D>();
            _deviceInput = GetComponent<IDeviceInput>();
        }


        private void FixedUpdate()
        {


            RotateShip(_deviceInput.ScanRotation());
            MoveShip(_deviceInput.ScanMove());
            TryTeleport(transform.position);

        }

        private void RotateShip(float rotationInput)
        {
            float rotationAngle = -rotationInput * _shipData.AngularSpeed * Time.fixedDeltaTime;
            _rbController.MoveRotation(_rbController.rotation + rotationAngle);
            _statisticsView.UpdateAngleRotation(_rbController.rotation);
        }

        private void MoveShip(float thrustInput)
        {
            if (thrustInput > 0)
            {
                Vector2 forwardForce = -transform.up * _shipData.Speed * thrustInput * Time.fixedDeltaTime;
                _rbController.linearVelocity = forwardForce;
            }
            _statisticsView.UpdateCoordinates(_rbController.position);
            _statisticsView.UpdateSpaceShipVelocity(_rbController.linearVelocity);
        }



        private void TryTeleport(Vector2 position)
        {
            Vector2 newPos = position;

            if (position.x < _shipData.DownLeftBorder.x)  
            {
                newPos = new Vector2(_shipData.UpRightBorder.x, position.y);
            }
            else if (position.x > _shipData.UpRightBorder.x)  
            {
                newPos = new Vector2(_shipData.DownLeftBorder.x, position.y);
            }

            else if (position.y < _shipData.DownLeftBorder.y)  
            {
                newPos = new Vector2(position.x, _shipData.UpRightBorder.y);
            }

            else if (position.y > _shipData.UpRightBorder.y) 
            {
                newPos = new Vector2(position.x, _shipData.DownLeftBorder.y);
            }
            transform.localPosition = newPos;
           
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseEnemy someEnemy = collision.GetComponent<BaseEnemy>();

            if (someEnemy && !gameObject.GetComponentInChildren<LaserBullet>())
            {
                Die();
            }
        }

        private void Die()
        {
            _statisticsController.gameObject.SetActive(true);
            _statisticsController.UpdateDestroyedEnemies();
            Destroy(gameObject);
        }

    }
}