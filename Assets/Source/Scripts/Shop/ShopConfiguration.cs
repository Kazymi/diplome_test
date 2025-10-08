using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create ShopConfiguration", fileName = "ShopConfiguration", order = 0)]
public class ShopConfiguration : ScriptableObject
{
    public int Price;
    public Sprite Sprite;
    [Range(0, 100)] public int SpawnChance = 50;
    public ShopBuyEvent BuyEvent;
}

public enum ShopBuyEvent
{
    Pet
}