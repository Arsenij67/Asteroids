using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(ShipStatisticsModel))]
public class ShipStatisticsController : MonoBehaviour
{
    private ShipStatisticsModel shipStModel;

    [SerializeField]private ShipStatisticsView shipStView;

    private void Awake()
    {
        shipStModel = GetComponent<ShipStatisticsModel>();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateDestroyedEnemies()
    {
        shipStView.UpdateDestoyedEnemies(shipStModel.enemiesDestroyed);
    }

   
}
