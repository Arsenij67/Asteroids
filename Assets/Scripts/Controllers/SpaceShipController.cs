using UnityEngine;
using UnityEngine.UIElements;

public class SpaceShipController : MonoBehaviour
{
    private SpaceShipData shipData;
    private Rigidbody2D rbController;

    [SerializeField] private ShipStatisticsView statisticsView;
    [SerializeField] ShipStatisticsController statisticsController;
    private void Awake()
    {
        shipData = GetComponent<SpaceShipData>();
        rbController = GetComponent<Rigidbody2D>();
    }
    

    private void FixedUpdate()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float thrustInput = Input.GetAxis("Vertical");

        RotateShip(rotationInput);
        MoveShip(thrustInput);
        TryTeleport(transform.position);

    }

    private void RotateShip(float rotationInput)
    {
        float rotationAngle = -rotationInput * shipData.AngularSpeed * Time.fixedDeltaTime;
        rbController.MoveRotation(rbController.rotation + rotationAngle);
        statisticsView.UpdateAngleRotation(rbController.rotation);
    }

    private void MoveShip(float thrustInput)
    {
        if (thrustInput > 0)
        {
            Vector2 forwardForce = -transform.up * shipData.Speed * thrustInput * Time.fixedDeltaTime;
            rbController.linearVelocity = forwardForce;
        }
        statisticsView.UpdateCoordinates(rbController.position);
        statisticsView.UpdateSpaceShipVelocity(rbController.linearVelocity);
    }

 

    private void TryTeleport(Vector2 position)
    {
        Vector2 newPos = position;

        if (position.x < shipData.DownLeftBorder.x) // если сдвинулись влево
        {
            newPos =  new Vector2(shipData.UpRightBorder.x,position.y);
        }
        else if (position.x > shipData.UpRightBorder.x) // если сдвинулись вправо
        {
            newPos = new Vector2(shipData.DownLeftBorder.x, position.y);
        }

        else if (position.y > shipData.DownLeftBorder.y) // если сдвинулись вверх
        {
            newPos = new Vector2(position.x,shipData.UpRightBorder.y);
        }

        else if (position.y < shipData.UpRightBorder.y) // если сдвинулись вниз
        {
            newPos = new Vector2(position.x, shipData.DownLeftBorder.y);
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
        statisticsController.gameObject.SetActive(true);
        statisticsController.UpdateDestroyedEnemies();
        Destroy(gameObject);
    }

}