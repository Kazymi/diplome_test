using UnityEngine;

// Пресеты для создания ShopConfiguration с артефактами
public static class ShopArtifactPresets
{
    public static ShopConfiguration CreateShopWithArtifact(ArtifactConfiguration artifact, int price, int spawnChance = 50)
    {
        var shopConfig = ScriptableObject.CreateInstance<ShopConfiguration>();
        shopConfig.Price = price;
        shopConfig.SpawnChance = spawnChance;
        shopConfig.Artifact = artifact;
        shopConfig.Sprite = artifact.Icon; // Используем иконку артефакта как иконку в магазине
        
        return shopConfig;
    }
}

