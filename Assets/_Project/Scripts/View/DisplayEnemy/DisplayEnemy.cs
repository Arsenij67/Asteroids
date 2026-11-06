using UnityEngine;

namespace Asteroid.Effects
{
public class DisplayEnemy : MonoBehaviour
{
    private static int _dieTrigger;

    private Animator _animationController;

    public void Initialize()
    {
        _dieTrigger = Animator.StringToHash("DieTrigger");
        _animationController = GetComponent<Animator>();
    }

    public void PlayDieEffect()
    {
        _animationController.SetTrigger(_dieTrigger);
    }
}
}
