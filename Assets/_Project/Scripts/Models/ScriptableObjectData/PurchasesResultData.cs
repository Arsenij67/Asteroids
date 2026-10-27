using Asteroid.UI;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopNotificationData", menuName = "Scriptable Objects/PurchasesResultData")]
public class PurchasesResultData : ScriptableObject
{
    [field: SerializeField] public Sprite NoAdsSprite { get; private set; }
    [field: SerializeField] public Sprite CoinSprite{ get; private set; }
    [field: SerializeField] public string TextForNoAds { get; private set; }
    [field: SerializeField] public string TextForCoins { get; private set; }
    [field: SerializeField] public PurchasesResultView WindowPrefab { get; private set; }
}
