using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create ArtifactConfiguration", fileName = "ArtifactConfiguration", order = 0)]
public class ArtifactConfiguration : ScriptableObject
{
    [Header("Basic Info")]
    public string ArtifactName;
    public Sprite Icon;
    public ArtifactRarity Rarity;
    
    [Header("Effects")]
    public ArtifactEffect[] Effects;

    // Генерирует полное описание артефакта со всеми эффектами
    public string GetFullDescription()
    {
        var sb = new StringBuilder();
        
        if (Effects != null && Effects.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Эффекты:");
            
            foreach (var effect in Effects)
            {
                sb.AppendLine($"• {GetEffectDescription(effect)}");
            }
        }
        
        return sb.ToString().TrimEnd();
    }

    // Получает описание конкретного эффекта
    private string GetEffectDescription(ArtifactEffect effect)
    {
        var effectName = GetEffectName(effect.EffectType);
        var value = effect.IsPercentage ? $"{effect.Value}%" : effect.Value.ToString();
        var condition = GetConditionDescription(effect.Condition);
        
        return $"{effectName}: +{value}{condition}";
    }

    // Возвращает читаемое название эффекта
    private string GetEffectName(ArtifactEffectType effectType)
    {
        return effectType switch
        {
            // Персонаж
            ArtifactEffectType.PlayerSpeed => "Скорость игрока",
            ArtifactEffectType.PlayerAttackSpeed => "Скорость атаки игрока",
            ArtifactEffectType.PlayerHealth => "Здоровье игрока",
            ArtifactEffectType.PlayerAttackDamage => "Урон игрока",
            ArtifactEffectType.PlayerCritChance => "Шанс критического удара",
            ArtifactEffectType.PlayerMissChance => "Шанс промаха",
            ArtifactEffectType.PlayerRegeneration => "Регенерация здоровья",
            ArtifactEffectType.PlayerLucky => "Удача",
            
            // Юниты
            ArtifactEffectType.UnitAttackSpeed => "Скорость атаки юнитов",
            ArtifactEffectType.UnitAttackDamage => "Урон юнитов",
            ArtifactEffectType.UnitSpeed => "Скорость юнитов",
            ArtifactEffectType.UnitAttackRange => "Дальность атаки юнитов",
            
            // Снаряды
            ArtifactEffectType.ProjectileCount => "Количество снарядов",
            
            // Магазин
            ArtifactEffectType.ShopPriceReduction => "Скидка в магазине",
            
            // Волны
            ArtifactEffectType.WaveMonsterCount => "Количество монстров в волне",
            ArtifactEffectType.WaveMonsterCountPercent => "Количество монстров в волне",
            
            // Специальные эффекты
            ArtifactEffectType.HealthLossPerSecond => "Потеря здоровья в секунду",
            ArtifactEffectType.DamageReductionOnHit => "Снижение урона при получении урона",
            
            _ => effectType.ToString()
        };
    }

    // Возвращает описание условия активации эффекта
    private string GetConditionDescription(ArtifactEffectCondition condition)
    {
        return condition switch
        {
            ArtifactEffectCondition.OnDamageReceived => " (при получении урона)",
            _ => ""
        };
    }
}

public enum ArtifactRarity
{
    Common,     // Обычный
    Rare,       // Редкий  
    Epic        // Очень редкий
}

