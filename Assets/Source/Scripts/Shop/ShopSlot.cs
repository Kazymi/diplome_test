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
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;

    private ShopConfiguration _shopConfiguration;
    public event Action<ShopSlot> OnPurchaseEvent;

    public ShopConfiguration ShopConfiguration => _shopConfiguration;
    public void Initialize(ShopConfiguration shopConfiguration)
    {
        triggerZone.OnTriggerEnterCompleted += Purchase;
        _shopConfiguration = shopConfiguration;
        _shopImage.sprite = shopConfiguration.Sprite;
        _price.text = shopConfiguration.Price.ToString();
        if (shopConfiguration.Artifact != null)
        {
            _shopImage.sprite = shopConfiguration.Artifact.Icon;
            _description.text = shopConfiguration.Artifact.GetFullDescription();
            _name.text = shopConfiguration.Artifact.ArtifactName;
        }
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