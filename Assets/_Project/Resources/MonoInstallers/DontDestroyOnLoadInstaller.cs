using Asteroid.Generation;
using UnityEngine;
using Zenject;

public class DontDestroyOnLoadInstaller : MonoInstaller
{
    [SerializeField] private GameObject [] _gameEntities;
    public override void InstallBindings()
    {
        foreach (var entity in _gameEntities)
        { 
            DontDestroyOnLoad(entity);
        }
    }
}