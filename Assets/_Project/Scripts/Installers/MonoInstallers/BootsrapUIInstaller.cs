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
            Container.Bind<Button>().FromInstance(_button).AsSingle().NonLazy();
            Container.Bind<Slider>().FromInstance(_slider).AsSingle().NonLazy();
            var bootstrapUI = Container.InstantiatePrefabForComponent<BootstrapUI>(this.gameObject);
            Container.Bind<BootstrapUI>().FromInstance(bootstrapUI).AsSingle().NonLazy();

        }
    }
}
