using UnityEngine;

[CreateAssetMenu(fileName = "BaseSceneData", menuName = "Scriptable Objects/BaseSceneData")]
public class BaseSceneData : ScriptableObject
{
    [field: SerializeField] public string SceneName { get; private set; }

    public readonly float timeWaitLoading = 2.00f;
    public readonly float finalLoadingShare = 0.9f;
    public readonly float finalProgressTasks = 1f;
}
