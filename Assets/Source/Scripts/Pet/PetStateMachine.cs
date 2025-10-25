using System;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;

public class PetStateMachine : MonoBehaviour
{
    [SerializeField] private PetConfiguration petConfiguration;
    [SerializeField] private PetParamSystem petParamSystem;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayerMask = 1 << 6; // слой врагов

    private Transform player;
    private StateMachine.StateMachine stateMachine;
    private PetAnimatorController petAnimatorController;
    private PetAttackState petAttackState;
    private PetChaseState petChaseState;
    private Transform currentTarget;

    private void Awake()
    {
        // Принудительно фиксируем поворот
        transform.rotation = Quaternion.identity;
        petParamSystem = FindFirstObjectByType<PetParamSystem>();
        Initialize(FindFirstObjectByType<PlayerStateMachine>().transform);
    }

    public void Initialize(Transform player)
    {
        this.player = player;
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        petAnimatorController = new PetAnimatorController(animator);

        var spawn = new PetAnimationState(PetAnimationType.Spawn, petAnimatorController);
        var idle = new PetAnimationState(PetAnimationType.Idle, petAnimatorController);
        var follow = new PetFollowState(petParamSystem, petAnimatorController, transform, player);
        petChaseState = new PetChaseState(petParamSystem, petAnimatorController, transform, null);
        petAttackState = new PetAttackState(petParamSystem, petAnimatorController, transform, null);

        spawn.AddTransition(new StateTransition(idle,
            new TemporaryCondition(petAnimatorController.GetAnimationDuration(PetAnimationType.Spawn))));
        idle.AddTransition(new StateTransition(follow, new TemporaryCondition(3f)));
        idle.AddTransition(new StateTransition(follow,
            new FuncCondition(() =>
                player != null && Vector2.Distance(transform.position, player.position) >
                petParamSystem.FollowDistance)));
        follow.AddTransition(new StateTransition(idle,
            new FuncCondition(() =>
                player != null &&
                Vector2.Distance(transform.position, player.position) <= petParamSystem.FollowDistance &&
                !HasEnemyInRange().Item1)));
        follow.AddTransition(new StateTransition(petChaseState,
            new FuncCondition(() =>
            {
                var returnValue = HasEnemyInRange();
                if (returnValue.Item1)
                {
                    petAttackState.SetTarget(returnValue.Item2);
                    petChaseState.SetTarget(returnValue.Item2);
                }

                return returnValue.Item1;
            })));

        petChaseState.AddTransition(new StateTransition(petAttackState,
            new FuncCondition(() =>
                currentTarget != null && Vector2.Distance(transform.position, currentTarget.position) <=
                petParamSystem.AttackRange)));

        petChaseState.AddTransition(new StateTransition(idle, new FuncCondition(() => petChaseState.Target == null)));
        petChaseState.AddTransition(new StateTransition(follow,
            new FuncCondition(() => !HasEnemyInRange().Item1)));

        var attackFinish = new TemporaryCondition(
            Mathf.Max(
                petAnimatorController.GetAnimationDuration(PetAnimationType.Attack),
                petParamSystem.AttackCooldown));

        petAttackState.AddTransition(new StateTransition(petChaseState, attackFinish));

        stateMachine = new StateMachine.StateMachine(spawn);
    }

    private void Update()
    {
        stateMachine.Tick();
        UpdateAttackTarget();
    }

    private (bool, Transform) HasEnemyInRange()
    {
        if (player == null) return (false, null);

        var enemies = Physics2D.OverlapCircleAll(player.position, petParamSystem.DetectionRadius, enemyLayerMask);
        var target = enemies.Length > 0 ? enemies[0].transform : null;
        return (enemies.Length > 0, target);
    }

    private void UpdateAttackTarget()
    {
        if (player == null) return;

        var enemies = Physics2D.OverlapCircleAll(player.position, petParamSystem.DetectionRadius, enemyLayerMask);

        if (enemies.Length > 0)
        {
            Transform closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                var distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            currentTarget = closestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    public void OnAttackHit()
    {
        petAttackState?.OnAttackHit();
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null && petParamSystem != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, petParamSystem.DetectionRadius);
        }
    }
}