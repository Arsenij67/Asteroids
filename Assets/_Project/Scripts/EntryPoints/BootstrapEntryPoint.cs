using UnityEngine;
using Zenject;

namespace Asteroid.Generation
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        [Inject] private RectTransform _userInterface;
        [Inject] private BootstrapController _bootstrapController;

        private void SetUpUI(RectTransform parent)
        {
            _bootstrapController.SetUpUI(parent);
        }
    }
}