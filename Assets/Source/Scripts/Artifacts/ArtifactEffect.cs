using UnityEngine;

[System.Serializable]
public class ArtifactEffect
{
    public ArtifactEffectType EffectType;
    public float Value;
    public bool IsPercentage;
    public ArtifactEffectCondition Condition;
}

public enum ArtifactEffectType
{
    // Персонаж
    PlayerSpeed,
    PlayerAttackSpeed,
    PlayerHealth,
    PlayerAttackDamage,
    PlayerCritChance,
    PlayerMissChance,
    PlayerRegeneration,
    PlayerLucky,
    
    // Юниты
    UnitAttackSpeed,
    UnitAttackDamage,
    UnitSpeed,
    UnitAttackRange,
    UnitCount,
    UnitCountPercent,
    
    // Снаряды
    ProjectileCount,
    
    // Магазин
    ShopPriceReduction,
    
    // Волны
    WaveMonsterCount,
    WaveMonsterCountPercent,
    
    // Специальные эффекты
    HealthLossPerSecond,
    DamageReductionOnHit
}

public enum ArtifactEffectCondition
{
    None,
    OnDamageReceived,
}

