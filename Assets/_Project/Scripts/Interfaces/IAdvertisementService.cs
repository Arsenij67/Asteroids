using UnityEngine;

namespace Asteroid.Services.UnityAdvertisement
{
    public interface IAdvertisementService 
    {
        public bool IsInitialized {  get; }  
        public bool IsLoaded {  get; }
        public bool IsShowed {  get; }
        public bool IsEnabled { get; }  
        public void Initialize(params object [] parameters);
        public void Load(params object[] parameters);
        public void Show(params object[] parameters);
    }
}
