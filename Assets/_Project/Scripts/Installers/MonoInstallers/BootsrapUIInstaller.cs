using Asteroid.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.Installers.MonoInstallers
{
    [RequireComponent(typeof(BootstrapUI))]
    public class BootsrapUIInstaller : MonoInstaller
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _button;
        public override void InstallBindings()
        {
            Container.Bind<Slider>().FromInstance(_slider).AsSingle();
            Container.Bind<Button>().FromInstance(_button).AsSingle();
            Container.Bind<BootstrapUI>().FromNewComponentOn(gameObject).AsSingle();
        }
    }
}
