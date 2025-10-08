using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private ShopConfiguration reloadConfiguration;
    [SerializeField] private ShopConfiguration[] shopConfigurations;
    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private ShopSlot reloadSlot;
    [SerializeField] private ShopSystem shop;
    private void Awake()
    {
        reloadSlot.Initialize(reloadConfiguration);
        reloadSlot.OnPurchaseEvent += OnReloadPurchaseEvent;
        foreach (var shopSlot in shopSlots)
        {
            var randomChance = GetRandomShopConfiguration();
            shopSlot.Initialize(randomChance);
            shopSlot.OnPurchaseEvent += OnPurchaseEvent;
        }
    }

    private void OnReloadPurchaseEvent(ShopSlot obj)
    {
        reloadSlot.GetComponent<TwoObjectsAnimator>().HideWithExplosion();
        foreach (var slot in shopSlots)
        {
            slot.GetComponent<TwoObjectsAnimator>().HideSimply();
        }
        DOVirtual.DelayedCall(1f, () =>
        {
            Destroy(gameObject);
            FindAnyObjectByType<EnemySpawner>().SpawnShop();

        });
    }

    private void OnPurchaseEvent(ShopSlot obj)
    {
        reloadSlot.GetComponent<TwoObjectsAnimator>().HideSimply();
        foreach (var slot in shopSlots)
        {
            Destroy(gameObject, 1f);
            if (slot == obj)
            {
                slot.GetComponent<TwoObjectsAnimator>().HideWithExplosion();
            }
            else
            {
                slot.GetComponent<TwoObjectsAnimator>().HideSimply();
            }
        }
    }

    //Генератор случайных обьектов относительно шанса
    private ShopConfiguration GetRandomShopConfiguration()
    {
        var totalChance = shopConfigurations.Aggregate(0f, (current, item) => current + item.SpawnChance);
        var randomValue = Random.Range(0f, totalChance);
        var currentSum = 0f;
        foreach (var item in shopConfigurations)
        {
            currentSum += item.SpawnChance;
            if (randomValue <= currentSum)
                return item;
        }

        return shopConfigurations[^1];
    }
}