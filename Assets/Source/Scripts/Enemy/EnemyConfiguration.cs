using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfiguration")]
public class EnemyConfiguration : ScriptableObject
{
    [Header("Movement")] 
    public float MoveSpeed = 2.5f;

    [Header("Combat")] 
    public int Damage = 10;
    public float AttackRange = 1.2f;
    public float AttackCooldown = 0.8f;

    [Header("Stats")] 
    public int Health = 10;
    
    [Header("Drop")]
    [Range(0,100)]
    public int DropChance = 50;
}