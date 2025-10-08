using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private TriggerZone triggerZone;
    [SerializeField] private Image _shopImage;
    [SerializeField] private TMP_Text _price;

    private ShopConfiguration _shopConfiguration;
    public event Action<ShopSlot> OnPurchaseEvent;

    public ShopConfiguration ShopConfiguration => _shopConfiguration;
    public void Initialize(ShopConfiguration shopConfiguration)
    {
        triggerZone.OnTriggerEnterCompleted += Purchase;
        _shopConfiguration = shopConfiguration;
        _shopImage.sprite = shopConfiguration.Sprite;
        _price.text = shopConfiguration.Price.ToString();
    }

    public void Purchase()
    {
        var counter = FindAnyObjectByType<CoinCounter>();
        if (counter.AmountCoin >= _shopConfiguration.Price)
        {
            counter.ReduceCoinCount(_shopConfiguration.Price);
            OnPurchaseEvent?.Invoke(this);
        }
    }
}