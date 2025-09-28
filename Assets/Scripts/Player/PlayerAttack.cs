using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Vector2 range;
    [SerializeField] private PlayerConfiguration playerConfiguration;
    [SerializeField] private Animator animator;
    private bool canAttach = true;

    private void OnDrawGizmosSelected()
    {
        if (pivot == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pivot.position, range * playerConfiguration.AttachRange);
    }

    private void FixedUpdate()
    {
        if (canAttach == false) return;
        var foundTargets = GetFoundColliders();
        if (foundTargets.Length == 0) return;
        foreach (var foundTarget in foundTargets)
        {
            foundTarget.GetComponent<IDamageable>().TakeDamage(playerConfiguration.AttackDamage);
            canAttach = false;
            animator.Play("Attack");
            DOVirtual.DelayedCall(playerConfiguration.AttackSpeed, () => { canAttach = true; });
        }
    }

    private Collider2D[] GetFoundColliders()
    {
        var colliders = Physics2D.OverlapBoxAll(pivot.position, range * playerConfiguration.AttachRange, 10)
            .Where(t =>
            {
                var damageable = t.GetComponent<IDamageable>();
                return damageable is { CanTakeDamage: true };
            }).ToArray();
        return colliders;
    }
}