using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(ObstaclesGenerationData))]
public class ObstaclesGenerationController : MonoBehaviour
{
    private ObstaclesGenerationData genData;
    private void Awake()
    {
        genData = GetComponent<ObstaclesGenerationData>();
    }

    private void Start()
    {
        StartCoroutine(WaitForNextGeneration());
    }

    private IEnumerator WaitForNextGeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(genData.GenFrequency);
            GenerateObstacle(genData.PrefabToGenerateNow);
        }
         
    }
    private void GenerateObstacle(BaseEnemy enemy)
    {
        if (genData.DirectionToFly)
        {
            BaseEnemy enemyScene = Instantiate(enemy,
                genData.PointToGenerate.position, Quaternion.identity);

            Vector2 direction = genData.DirectionToFly.position - enemyScene.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            enemyScene.rb2dEnemy.MoveRotation(angle);
        }
    }


}
