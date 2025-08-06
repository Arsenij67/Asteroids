using UnityEngine;

namespace Asteroid.UnityAdvertisement
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
