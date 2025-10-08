using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
public class PlayerConfiguration : ScriptableObject
{
    [field: SerializeField] public float DefaultSpeed { get; private set; }
    [field: SerializeField] public float DefaultAttackSpeed { get; private set; }
    [field: SerializeField] public float DefaultAttachRange { get; private set; }
    [field: SerializeField] public int DefaultAttackDamage { get; private set; }
    [field: SerializeField] public int DefaultAttachDamage { get; private set; }
    [field: SerializeField] public int DefaultCritChance { get; private set; }
    [field: SerializeField] public int DefaultArmore { get; private set; }
    [field: SerializeField] public int DefaultMiss { get; private set; }
    [field: SerializeField] public int DefaultRegeneration { get; private set; }
    [field: SerializeField] public int DefaultLucky { get; private set; }
    [field: SerializeField] public int DefaultMoneyPerLevel { get; private set; }
    [field: SerializeField] public int DefaultLuckyChest { get; private set; }
}