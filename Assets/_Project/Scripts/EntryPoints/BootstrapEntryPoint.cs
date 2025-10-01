using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        [Inject] private RectTransform _userInterface;
        [Inject] private BootstrapController _bootstrapController;

        private void Start()
        {
            _bootstrapController.OnGameStarted += () => Debug.Log("BootstrapEntryPoint в курсе");
        }

        private void SetUpUI(RectTransform parent)
        {
            _bootstrapController.SetUpUI(parent);
        }
    }
}