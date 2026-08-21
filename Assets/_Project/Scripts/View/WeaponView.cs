using Asteroid.Enemies;
using Asteroid.Generation;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Asteroid.UI
{
    public class WeaponView : MonoBehaviour
    {
        public  Action OnButtonFireballClicked;
        public  Action OnButtonLaserClicked;

        [SerializeField] private Button _fireballButton;

        [SerializeField] private Button _laserButton;

        public void Initialize()
        {
            _fireballButton.onClick.AddListener(NotifyAboutClickFireball);
            _laserButton.onClick.AddListener(NotifyAboutClickLaser);
        }

        private void NotifyAboutClickLaser()
        {
           OnButtonLaserClicked.Invoke();
        }

        private void NotifyAboutClickFireball()
        {
            OnButtonFireballClicked.Invoke();
        }

        private void OnDestroy()
        {
            _fireballButton.onClick.RemoveListener(NotifyAboutClickFireball);
            _laserButton.onClick.RemoveListener(NotifyAboutClickLaser);
        }
    }
}
