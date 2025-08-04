using UnityEngine;

namespace Asteroid.UnityAdvertisement
{
    public class AdvertisementController 
    {
        private const string INTERSTITIAL = "Interstitial_Android";

        private string currentPlatform = string.Empty;

        private IAdvertisementService _advertisementService;
        public void Initialize(IAdvertisementService advertisementService)
        { 
            _advertisementService = advertisementService;
        }

        public void PlayerDieHandler()
        {
            if (_advertisementService.isInitialized)
            {
                _advertisementService.Show(INTERSTITIAL);
            }
        }

        private void CheckPlatform()
        { 
            
        }

    }
}
