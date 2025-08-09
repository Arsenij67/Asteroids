using UnityEngine;

namespace Asteroid.Services.UnityAdvertisement
{
    public interface IAdvertisementService 
    {
        public bool isInitialized {  get; }  
        public bool isLoaded {  get; }
        public bool isShowed {  get; }
        public void Initialize(params object [] parameters);
        public void Load(params object[] parameters);
        public void Show(params object[] parameters);
    }
}
