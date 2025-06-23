
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "BootstrapSceneModel", menuName = "Scriptable Objects/BootstrapSceneModel")]
public class BootstrapSceneModel : ScriptableObject
{
    [field: SerializeField] public string ScenePreLoad { get; private set; }
    [field: SerializeField] public string BootstrapSceneName { get; private set; }

    public readonly float timeWaitLoading = 2.00f;
    public readonly float finalLoadingShare = 0.9f;
    public readonly float finalProgressTasks = 1f;
}
