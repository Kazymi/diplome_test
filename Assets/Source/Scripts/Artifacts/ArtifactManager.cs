using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] private CharacterParamSystem characterParamSystem;
    
    private List<ArtifactConfiguration> activeArtifacts = new List<ArtifactConfiguration>();
    private Dictionary<ArtifactEffectType, float> effectValues = new Dictionary<ArtifactEffectType, float>();
    private Dictionary<ArtifactEffectType, float> effectPercentages = new Dictionary<ArtifactEffectType, float>();
    
    // Специальные эффекты
    private bool healthLossActive;
    private float healthLossPerSecond;
    private float lastHealthLossTime;
    
    private void Start()
    {
        InitializeEffectDictionaries();
    }
    
    private void Update()
    {
        HandleHealthLossEffect();
    }
    
    private void InitializeEffectDictionaries()
    {
        var effectTypes = System.Enum.GetValues(typeof(ArtifactEffectType));
        foreach (ArtifactEffectType effectType in effectTypes)
        {
            effectValues[effectType] = 0f;
            effectPercentages[effectType] = 0f;
        }
    }
    
    public void AddArtifact(ArtifactConfiguration artifact)
    {
        activeArtifacts.Add(artifact);
        ApplyArtifactEffects(artifact);
    }
    
    public void RemoveArtifact(ArtifactConfiguration artifact)
    {
        if (activeArtifacts.Contains(artifact))
        {
            RemoveArtifactEffects(artifact);
            activeArtifacts.Remove(artifact);
        }
    }
    
    private void ApplyArtifactEffects(ArtifactConfiguration artifact)
    {
        foreach (var effect in artifact.Effects)
        {
            ApplyEffect(effect);
        }
    }
    
    private void RemoveArtifactEffects(ArtifactConfiguration artifact)
    {
        foreach (var effect in artifact.Effects)
        {
            RemoveEffect(effect);
        }
    }
    
    private void ApplyEffect(ArtifactEffect effect)
    {
        if (effect.IsPercentage)
        {
            effectPercentages[effect.EffectType] += effect.Value;
        }
        else
        {
            effectValues[effect.EffectType] += effect.Value;
        }
        
        // Применяем эффект к персонажу
        ApplyEffectToCharacter(effect);
        
        // Обрабатываем специальные эффекты
        HandleSpecialEffects(effect);
    }
    
    private void RemoveEffect(ArtifactEffect effect)
    {
        if (effect.IsPercentage)
        {
            effectPercentages[effect.EffectType] -= effect.Value;
        }
        else
        {
            effectValues[effect.EffectType] -= effect.Value;
        }
        
        // Убираем эффект с персонажа
        RemoveEffectFromCharacter(effect);
    }
    
    private void ApplyEffectToCharacter(ArtifactEffect effect)
    {
        switch (effect.EffectType)
        {
            case ArtifactEffectType.PlayerSpeed:
                if (effect.IsPercentage)
                    characterParamSystem.AddSpeedPercentBonus(effect.Value);
                else
                    characterParamSystem.AddSpeedBonus(effect.Value);
                break;
                
            case ArtifactEffectType.PlayerAttackSpeed:
                if (effect.IsPercentage)
                    characterParamSystem.AddAttackSpeedPercentBonus(effect.Value);
                else
                    characterParamSystem.AddAttackSpeedBonus(effect.Value);
                break;
                
            case ArtifactEffectType.PlayerHealth:
                if (effect.IsPercentage) characterParamSystem.AddHealthPercentBonus(effect.Value);
                else
                    characterParamSystem.AddHealthBonus(effect.Value);
                break;
                
            case ArtifactEffectType.PlayerAttackDamage:
                if (effect.IsPercentage)
                    characterParamSystem.AddAttackDamagePercentBonus(effect.Value);
                else
                    characterParamSystem.AddAttackDamageBonus((int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerCritChance:
                if (effect.IsPercentage)
                    characterParamSystem.AddCritChancePercentBonus(effect.Value);
                else
                    characterParamSystem.AddCritChanceBonus((int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerMissChance:
                if (effect.IsPercentage)
                    characterParamSystem.AddMissPercentBonus(effect.Value);
                else
                    characterParamSystem.AddMissBonus((int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerRegeneration:
                if (effect.IsPercentage)
                    characterParamSystem.AddRegenerationPercentBonus(effect.Value);
                else
                    characterParamSystem.AddRegenerationBonus((int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerLucky:
                if (effect.IsPercentage)
                    characterParamSystem.AddLuckyPercentBonus(effect.Value);
                else
                    characterParamSystem.AddLuckyBonus((int)effect.Value);
                break;
        }
    }
    
    private void RemoveEffectFromCharacter(ArtifactEffect effect)
    {
        switch (effect.EffectType)
        {
            case ArtifactEffectType.PlayerSpeed:
                if (effect.IsPercentage)
                    characterParamSystem.AddSpeedPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddSpeedBonus(-effect.Value);
                break;
                
            case ArtifactEffectType.PlayerAttackSpeed:
                if (effect.IsPercentage)
                    characterParamSystem.AddAttackSpeedPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddAttackSpeedBonus(-effect.Value);
                break;
                
            case ArtifactEffectType.PlayerHealth:
                if (effect.IsPercentage)
                    characterParamSystem.AddHealthPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddHealthBonus(-effect.Value);
                break;
                
            case ArtifactEffectType.PlayerAttackDamage:
                if (effect.IsPercentage)
                    characterParamSystem.AddAttackDamagePercentBonus(-effect.Value);
                else
                    characterParamSystem.AddAttackDamageBonus(-(int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerCritChance:
                if (effect.IsPercentage)
                    characterParamSystem.AddCritChancePercentBonus(-effect.Value);
                else
                    characterParamSystem.AddCritChanceBonus(-(int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerMissChance:
                if (effect.IsPercentage)
                    characterParamSystem.AddMissPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddMissBonus(-(int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerRegeneration:
                if (effect.IsPercentage)
                    characterParamSystem.AddRegenerationPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddRegenerationBonus(-(int)effect.Value);
                break;
                
            case ArtifactEffectType.PlayerLucky:
                if (effect.IsPercentage)
                    characterParamSystem.AddLuckyPercentBonus(-effect.Value);
                else
                    characterParamSystem.AddLuckyBonus(-(int)effect.Value);
                break;
        }
    }
    
    private void HandleSpecialEffects(ArtifactEffect effect)
    {
        switch (effect.EffectType)
        {
            case ArtifactEffectType.HealthLossPerSecond:
                healthLossActive = true;
                healthLossPerSecond = effect.Value;
                break;
        }
    }
    
    private void HandleHealthLossEffect()
    {
        if (healthLossActive && Time.time - lastHealthLossTime >= 1f)
        {
            lastHealthLossTime = Time.time;
        }
    }
    
    public float GetUnitAttackSpeedBonus() => effectValues[ArtifactEffectType.UnitAttackSpeed];
    public float GetUnitAttackSpeedPercentBonus() => effectPercentages[ArtifactEffectType.UnitAttackSpeed];
    public float GetUnitAttackDamageBonus() => effectValues[ArtifactEffectType.UnitAttackDamage];
    public float GetUnitAttackDamagePercentBonus() => effectPercentages[ArtifactEffectType.UnitAttackDamage];
    public float GetUnitSpeedBonus() => effectValues[ArtifactEffectType.UnitSpeed];
    public float GetUnitSpeedPercentBonus() => effectPercentages[ArtifactEffectType.UnitSpeed];
    public float GetUnitAttackRangeBonus() => effectValues[ArtifactEffectType.UnitAttackRange];
    public float GetUnitAttackRangePercentBonus() => effectPercentages[ArtifactEffectType.UnitAttackRange];
    public int GetProjectileCountBonus() => (int)effectValues[ArtifactEffectType.ProjectileCount];
    public float GetShopPriceReduction() => effectPercentages[ArtifactEffectType.ShopPriceReduction];
    public int GetWaveMonsterCountBonus() => (int)effectValues[ArtifactEffectType.WaveMonsterCount];
    public float GetWaveMonsterCountPercentBonus() => effectPercentages[ArtifactEffectType.WaveMonsterCountPercent];
    
    public void OnPlayerTakeDamage()
    {
        foreach (var artifact in activeArtifacts)
        {
            foreach (var effect in artifact.Effects)
            {
                if (effect.Condition == ArtifactEffectCondition.OnDamageReceived)
                {
                    HandleConditionalEffect(effect);
                }
            }
        }
    }
    
    private void HandleConditionalEffect(ArtifactEffect effect)
    {
        switch (effect.EffectType)
        {
            case ArtifactEffectType.ProjectileCount:
            case ArtifactEffectType.PlayerCritChance:
                if (effect.Condition == ArtifactEffectCondition.OnDamageReceived)
                {
                    RemoveEffect(effect);
                }
                break;
                
            case ArtifactEffectType.DamageReductionOnHit:
                if (effect.Condition == ArtifactEffectCondition.OnDamageReceived)
                {
                    effectValues[ArtifactEffectType.UnitAttackDamage] -= effect.Value;
                }
                break;
        }
    }
}
