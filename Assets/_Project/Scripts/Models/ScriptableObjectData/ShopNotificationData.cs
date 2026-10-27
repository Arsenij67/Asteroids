using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopNotificationData", menuName = "Scriptable Objects/ShopNotificationData")]
public class ShopNotificationData : ScriptableObject
{
    [field: SerializeField] public Sprite NoAdsSprite { get; private set; }
    [field: SerializeField] public Sprite CoinSprite{ get; private set; }

    [field: SerializeField] public string textForNoAds { get; private set; }
    [field: SerializeField] public string textForCoins { get; private set; }
}
