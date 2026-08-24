using UnityEngine;

namespace Asteroid.Effects
{ 

[RequireComponent(typeof(Animator))]
public class DisplayEnemy : MonoBehaviour
{
    private static int _dieTrigger;

    private Animator _animationController;

    public  void Initialize()
    { 
        _animationController = GetComponent<Animator>();   
        _dieTrigger = Animator.StringToHash("DieTrigger");
    }

    public void PlayDieEffect()
    {
        _animationController.SetTrigger(_dieTrigger);
    }

}
}
