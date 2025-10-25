using UnityEngine;

public class PetParamSystem : MonoBehaviour
{
    [SerializeField] private PetConfiguration petConfiguration;

    // Абсолютные бонусы
    private float followDistanceBonus;
    private float followSpeedBonus;
    private int damageBonus;
    private float attackRangeBonus;
    private float attackCooldownBonus;
    private float detectionRadiusBonus;

    // Процентные бонусы (в процентах, например 10 = +10%)
    private float followDistancePercentBonus;
    private float followSpeedPercentBonus;
    private float damagePercentBonus;
    private float attackRangePercentBonus;
    private float attackCooldownPercentBonus;
    private float detectionRadiusPercentBonus;

    // Геттеры с бонусами (базовое значение + абсолютный бонус) * (1 + процентный бонус/100)
    public float FollowDistance => (petConfiguration.FollowDistance + followDistanceBonus) * 
                                  (1 + followDistancePercentBonus / 100f);

    public float FollowSpeed => (petConfiguration.FollowSpeed + followSpeedBonus) * 
                                (1 + followSpeedPercentBonus / 100f);

    public int Damage => Mathf.RoundToInt((petConfiguration.Damage + damageBonus) * 
                                         (1 + damagePercentBonus / 100f));

    public float AttackRange => (petConfiguration.AttackRange + attackRangeBonus) * 
                                (1 + attackRangePercentBonus / 100f);

    public float AttackCooldown => (petConfiguration.AttackCooldown + attackCooldownBonus) * 
                                   (1 + attackCooldownPercentBonus / 100f);

    public float DetectionRadius => (petConfiguration.DetectionRadius + detectionRadiusBonus) * 
                                    (1 + detectionRadiusPercentBonus / 100f);

    // Методы для добавления абсолютных бонусов
    public void AddFollowDistanceBonus(float bonus) => followDistanceBonus += bonus;
    public void AddFollowSpeedBonus(float bonus) => followSpeedBonus += bonus;
    public void AddDamageBonus(int bonus) => damageBonus += bonus;
    public void AddAttackRangeBonus(float bonus) => attackRangeBonus += bonus;
    public void AddAttackCooldownBonus(float bonus) => attackCooldownBonus += bonus;
    public void AddDetectionRadiusBonus(float bonus) => detectionRadiusBonus += bonus;

    // Методы для добавления процентных бонусов
    public void AddFollowDistancePercentBonus(float percent) => followDistancePercentBonus += percent;
    public void AddFollowSpeedPercentBonus(float percent) => followSpeedPercentBonus += percent;
    public void AddDamagePercentBonus(float percent) => damagePercentBonus += percent;
    public void AddAttackRangePercentBonus(float percent) => attackRangePercentBonus += percent;
    public void AddAttackCooldownPercentBonus(float percent) => attackCooldownPercentBonus += percent;
    public void AddDetectionRadiusPercentBonus(float percent) => detectionRadiusPercentBonus += percent;

    // Метод для сброса всех бонусов
    public void ResetAllBonuses()
    {
        // Сброс абсолютных бонусов
        followDistanceBonus = followSpeedBonus = attackRangeBonus = attackCooldownBonus = detectionRadiusBonus = 0f;
        damageBonus = 0;

        // Сброс процентных бонусов
        followDistancePercentBonus = followSpeedPercentBonus = damagePercentBonus = 
            attackRangePercentBonus = attackCooldownPercentBonus = detectionRadiusPercentBonus = 0f;
    }
}
