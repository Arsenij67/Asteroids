using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameOverView : MonoBehaviour
{
    [field: SerializeField] public Button ButtonRestart { get; private set; }
    [field: SerializeField] public Button ButtonShowAd { get; private set; }
    [field: SerializeField] public TMP_Text EnemiesDestroyedText { get; private set; }
}
