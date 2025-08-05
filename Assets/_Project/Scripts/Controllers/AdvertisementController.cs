using System;

namespace Asteroid.UnityAdvertisement
{
    public class AdvertisementController 
    {
        private const string INTERSTITIAL_ANDROID = "Interstitial_Android";
        private const string REWARDED_ANDROID= "Rewarded_Android";
        private const string BANNER_ANDROID= "Banner_Android";
        private const string INTERSTITIAL_IOS = "Interstitial_iOS";
        private const string REWARDED_IOS = "Rewarded_iOS";
        private const string BANNER_IOS = "Banner_iOS";

        public event Action OnPlayerRevived;

        private IAdvertisementService _advertisementService;
        public void Initialize(IAdvertisementService advertisementService)
        { 
            _advertisementService = advertisementService;
            _advertisementService.Load(REWARDED_ANDROID);
        }

        public void ShowRewardedAd()
        {
            ShowAnyAd(REWARDED_ANDROID);
            OnPlayerRevived?.Invoke();
        }

        public void ShowInterstitialAd()
        {
            ShowAnyAd(INTERSTITIAL_ANDROID);
        }

        private void ShowAnyAd(string advertisementId)
        {
            if (!_advertisementService.isLoaded)
            {
                _advertisementService.Load(advertisementId);
            }
            _advertisementService.Show(advertisementId);
        }
    }
}
