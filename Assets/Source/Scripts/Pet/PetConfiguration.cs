using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PetConfiguration")]
public class PetConfiguration : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed = 3f;
    public float FollowDistance = 2f;
    public float FollowSpeed = 2.5f;
    
    [Header("Combat")]
    public int Damage = 5;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 1f;
    public float DetectionRadius = 4f;
    
    [Header("Behavior")]
    public float IdleTime = 2f;
    public float AttackMoveSpeed = 4f;
}
