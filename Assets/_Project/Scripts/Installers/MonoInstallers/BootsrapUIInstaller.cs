using Asteroid.Exit;
using Asteroid.Generation;
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
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonExit;
        [SerializeField] private RectTransform UIParent;

        public override void InstallBindings()
        {
            Container.Bind<Slider>().WithId("loadingSlider").FromInstance(_slider);
            Container.Bind<Button>().WithId("buttonStart").FromInstance(_buttonStart);
            Container.Bind<Button>().WithId("buttonExit").FromInstance(_buttonExit);
            Container.Bind<BootstrapUI>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<BootstrapController>().AsSingle();
            Container.Bind<RectTransform>().FromInstance(UIParent).AsSingle();
            
        }
    }
}
