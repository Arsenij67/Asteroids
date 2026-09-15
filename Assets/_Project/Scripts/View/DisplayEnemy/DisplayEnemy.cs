using Asteroid.Enemies;
using Asteroid.Generation;
using System;
using UnityEngine;

namespace Asteroid.Effects
{
public class DisplayEnemy: IDisposable
{
    private static int _dieTrigger;

    private Animator _animationController;
    private IResourceLoader _resourceLoader;

    public void Initialize(IResourceLoader resourceLoader, Animator animatorComponent, Enemy enemyTransform)
    {
        _resourceLoader = resourceLoader;
        _animationController = animatorComponent;
        _dieTrigger = Animator.StringToHash("DieTrigger");
    
    }

    public void PlayDieEffect()
    {
        _animationController.SetTrigger(_dieTrigger);
    }

    public void Dispose()
    { 
        _resourceLoader.UnloadResource(_animationController.gameObject.name);
    }
}
}
