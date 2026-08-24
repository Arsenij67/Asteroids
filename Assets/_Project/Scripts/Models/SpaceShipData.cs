using Asteroid.Database;
using Asteroid.Services.RemoteConfig;
using UnityEngine;

namespace Asteroid.SpaceShip
{
    [CreateAssetMenu(fileName = "SpaceShipData", menuName = "ScriptableObjects/SpaceShipData")]
    public class SpaceShipData:ScriptableObject
    {
        [SerializeField] private AssignmentMode _assignmentMode;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _speed;
        [SerializeField] private float _health;

        private RemoteConfigShip _remoteConfigShip;
        private IRemoteConfigService _remoteConfigService;
        public float AngularSpeed 
        {

            get
            {
                if (_assignmentMode.Equals(AssignmentMode.RemoteConfig))
                {
                    string shipJson = _remoteConfigService.GetValue<string>("ship_config");
                    _remoteConfigShip = JsonUtility.FromJson<RemoteConfigShip>(shipJson);
                    return _remoteConfigShip.AngularSpeed;
                }
                else
                {

                    return _angularSpeed;
                }
            }
        }

        public float Speed 
        {

            get
            {
                if (_assignmentMode.Equals(AssignmentMode.RemoteConfig))
                { 
                    string shipJson = _remoteConfigService.GetValue<string>("ship_config");
                    _remoteConfigShip = JsonUtility.FromJson<RemoteConfigShip>(shipJson);
                    return _remoteConfigShip.Speed;
                }

                else
                {

                    return _speed;
                }
            }
    
        }

        public float Health => _health;

        public void Initialize(IRemoteConfigService remoteConfigService)
        { 
            _remoteConfigService = remoteConfigService;    
        }

    }
}