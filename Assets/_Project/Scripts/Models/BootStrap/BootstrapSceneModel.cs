
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "BootstrapSceneModel", menuName = "Scriptable Objects/BootstrapSceneModel")]
public class BootstrapSceneModel : ScriptableObject
{
    [field: SerializeField] public string PreLoadedScene { get; private set; }

    public readonly float TIME_WAIT_LOADING = 2.00f;
}
