using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroid.Installers
{
    [RequireComponent(typeof(ShopUI))]
public class ShopUIInstaller : MonoInstaller
{
    [SerializeField] private TMP_Text _textCoins;
    [SerializeField] private Button _buttonBuy100Coins;
    [SerializeField] private Button _buttonBuyNoAds;
        public override void InstallBindings()
        {

            Container.Bind<ShopUI>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<Button>().WithId("buttonBuyNoAds").FromInstance(_buttonBuyNoAds);
            Container.Bind<Button>().WithId("buttonBuy100Coins").FromInstance(_buttonBuy100Coins);
            Container.Bind<TMP_Text>().FromInstance(_textCoins);
           
        }
}
}