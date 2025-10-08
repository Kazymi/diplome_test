using StateMachine;
using UnityEngine;

public class PetAttackState : State
{
    private readonly PetConfiguration config;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private Transform target;
    private readonly SpriteRenderer spriteRenderer;
    private float attackWindowTimer;

    public PetAttackState(PetConfiguration config, PetAnimatorController animatorController, Transform pet, Transform target)
    {
        this.config = config;
        this.animatorController = animatorController;
        this.pet = pet;
        this.target = target;
        this.spriteRenderer = pet.GetComponent<SpriteRenderer>();
    }

    public override void OnStateEnter()
    {
        animatorController.SetTrigger(PetAnimationType.Attack);
        attackWindowTimer = animatorController.GetAnimationDuration(PetAnimationType.Attack);
        if (attackWindowTimer < config.AttackCooldown)
            attackWindowTimer = config.AttackCooldown;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public override void Tick()
    {
        if (target == null) return;

        // Только атакуем, не двигаемся
        var dir = (target.position - pet.position).normalized;
        UpdateFacing(dir);
        attackWindowTimer -= Time.deltaTime;
    }

    private void UpdateFacing(Vector3 direction)
    {
        if (spriteRenderer == null) return;
        
        if (direction.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (direction.x < -0.01f)
            spriteRenderer.flipX = true;
    }

    public void OnAttackHit()
    {
        if (target == null) return;

        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(config.Damage);
        }
    }
}
