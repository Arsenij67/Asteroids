using Asteroid.Database;
using Asteroid.Services.RemoteConfig;
using UnityEngine;

namespace Asteroid.SpaceShip
{
    [CreateAssetMenu(fileName = "SpaceShipData", menuName = "ScriptableObjects/SpaceShipData")]
    public class SpaceShipData:ScriptableObject
    {
        private RemoteConfigShip _remoteConfigShip;
        private IRemoteConfigService _remoteConfigService;
        public float AngularSpeed 
        {

            get
            {
                string shipJson = _remoteConfigService.GetValue<string>("ship_config");
               _remoteConfigShip = JsonUtility.FromJson<RemoteConfigShip>(shipJson);
                return _remoteConfigShip.AngularSpeed;
            }
        }
        public float Speed 
        {

            get
            {
                string shipJson = _remoteConfigService.GetValue<string>("ship_config");
                _remoteConfigShip = JsonUtility.FromJson<RemoteConfigShip>(shipJson);
                return _remoteConfigShip.AngularSpeed;
            }
    
        }

        public void Initialize(IRemoteConfigService remoteConfigService)
        { 
            _remoteConfigService = remoteConfigService;    
        }

    }
}