using TMPro;
using UnityEngine;

public class ShipStatisticsView : MonoBehaviour
{
    [Header("UI Ссылки")]
    [SerializeField] private TMP_Text fireballCountText;
    [SerializeField] private TMP_Text laserCountText;
    [SerializeField] private TMP_Text coordinatesText;
    [SerializeField] private TMP_Text angleRotationText;
    [SerializeField] private TMP_Text rollbackTimeText;
    [SerializeField] private TMP_Text spaceShipVelocityText;
    [SerializeField] private TMP_Text enemiesDestroyedText;

    // Методы для обновления значений
    public void UpdateFireballCount(int count)
    {
        if (fireballCountText != null)
        {
            fireballCountText.text = $"Fireballs: {count}";
        }
    }

    public void UpdateLaserCount(int count)
    {
        if (laserCountText != null)
        {
            laserCountText.text = $"Lasers: {count}";
        }
    }

    public void UpdateCoordinates(Vector3 position)
    {
        if (coordinatesText != null)
        {
            coordinatesText.text = $"Pos: {position.x:F1}, {position.y:F1}";
        }
    }

    public void UpdateAngleRotation(float angle)
    {
        if (angleRotationText != null)
            angleRotationText.text = $"Angle: {angle:F1}°";
    }

    public void UpdateRollbackTime(float time)
    {
        if (rollbackTimeText != null)
        {
            rollbackTimeText.text = $"Rollback: {time:F1}s";
        }
    }

    public void UpdateSpaceShipVelocity(Vector3 velocity)
    {
        if (spaceShipVelocityText != null)
        {
            spaceShipVelocityText.text = $"Speed: {velocity.magnitude:F1} m/s";
        }
    }

    public void UpdateDestoyedEnemies(int count)
    {

        if (enemiesDestroyedText != null)
        {
            enemiesDestroyedText.text = $"Enemies destroyed: {count:D1} units";
        }
    }
}