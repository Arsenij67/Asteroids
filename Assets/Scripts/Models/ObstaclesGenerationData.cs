using UnityEngine;

public class ObstaclesGenerationData : MonoBehaviour
{
    [SerializeField] private Transform [] generationPoints;
    [SerializeField] private GameObject [] generationPrefabs;
    [SerializeField] private float generationFrequency = 2f; // частота генерации в секундах
    [SerializeField] private Transform directionToFly;

    public float GenFrequency => generationFrequency;
    public Transform [] GenerationPoints => generationPoints;

    public BaseEnemy PrefabToGenerateNow =>  generationPrefabs[Random.Range(0,generationPrefabs.Length)].GetComponent<BaseEnemy>();

    public Transform PointToGenerate => generationPoints[Random.Range(0, generationPoints.Length)];

    public Transform DirectionToFly => directionToFly;
}
