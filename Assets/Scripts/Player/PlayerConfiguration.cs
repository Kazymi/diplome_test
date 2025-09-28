using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
public class PlayerConfiguration : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttachRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
}