using Asteroid.Services;
using Asteroid.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
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
            SetUpUI(_userInterface);
            _bootstrapController.OnGameStarted += () => Debug.Log("BootstrapEntryPoint в курсе");
        }

        private void SetUpUI(RectTransform parent)
        {
            _bootstrapController.SetUpUI(parent);
        }
    }
}