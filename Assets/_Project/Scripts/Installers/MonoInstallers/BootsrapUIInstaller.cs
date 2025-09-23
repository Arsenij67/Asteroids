using Asteroid.Generation;
using Asteroid.UI;
using TMPro;
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
        [SerializeField] private TMP_Text _textCoins;
        [SerializeField] private Button _buttonBuy100Coins;
        public override void InstallBindings()
        {
            Container.Bind<Slider>().WithId("loadingSlider").FromInstance(_slider);
            Container.Bind<Button>().WithId("buttonStart").FromInstance(_buttonStart);
            Container.Bind<Button>().WithId("buttonBuyNoAds").FromInstance(_buttonBuyNoAds);
            Container.Bind<Button>().WithId("buttonBuy100Coins").FromInstance(_buttonBuy100Coins);
 
            Container.Bind<TMP_Text>().FromInstance(_textCoins);
            Container.Bind<BootstrapUI>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<BootstrapController>().AsSingle();
            Container.Bind<RectTransform>().FromInstance(UIParent).AsSingle();
            
        }
    }
}
