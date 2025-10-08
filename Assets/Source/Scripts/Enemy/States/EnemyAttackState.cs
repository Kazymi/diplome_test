using StateMachine;
using UnityEngine;

public class EnemyAttackState : State
{
    private readonly EnemyConfiguration config;
    private readonly EnemyAnimatorController animatorController;
    private readonly Transform self;
    private readonly Transform target;

    private float attackWindowTimer;

    public EnemyAttackState(EnemyConfiguration config, EnemyAnimatorController animatorController, Transform self,
        Transform target)
    {
        this.config = config;
        this.animatorController = animatorController;
        this.self = self;
        this.target = target;
    }

    public override void OnStateEnter()
    {
        animatorController.SetTrigger(EnemyAnimationType.Attack);
        attackWindowTimer = animatorController.GetAnimationDuration(EnemyAnimationType.Attack);
        if (attackWindowTimer < config.AttackCooldown)
            attackWindowTimer = config.AttackCooldown;
    }

    public override void Tick()
    {
        attackWindowTimer -= Time.deltaTime;
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