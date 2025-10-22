
using UnityEngine;

namespace Asteroid.Generation

{

    [CreateAssetMenu(fileName = "BootstrapSceneModel", menuName = "Scriptable Objects/BootstrapSceneModel")]
    public class BootstrapSceneData : BaseSceneData
    {
        [field: SerializeField] public string SceneGame { get; private set; }
    }
}
