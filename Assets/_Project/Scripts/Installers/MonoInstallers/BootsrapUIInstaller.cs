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
        [SerializeField] private Button _buttonBuyNoAds;
        [SerializeField] private RectTransform UIParent;
        public override void InstallBindings()
        {
            Container.Bind<Slider>().WithId("loadingSlider").FromInstance(_slider);
            Container.Bind<Button>().WithId("buttonStart").FromInstance(_buttonStart);
            Container.Bind<Button>().WithId("buttonBuyNoAds").FromInstance(_buttonBuyNoAds);
            Container.Bind<BootstrapUI>().FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<RectTransform>().FromInstance(UIParent).AsSingle();
            
        }
    }
}
