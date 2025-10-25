using StateMachine;
using UnityEngine;

public class PetFollowState : State
{
    private readonly PetParamSystem paramSystem;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private readonly Transform player;
    private readonly SpriteRenderer spriteRenderer;

    private float targetUpdateTimer;
    private float targetUpdateInterval = 2f;
    private Vector3 randomTargetPosition;
    private float currentTime;
    private float lastFlipTime;
    private const float flipCooldown = 0.1f;

    public PetFollowState(PetParamSystem paramSystem, PetAnimatorController animatorController, Transform pet,
        Transform player)
    {
        this.paramSystem = paramSystem;
        this.animatorController = animatorController;
        this.pet = pet;
        this.player = player;
        this.spriteRenderer = pet.GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnStateEnter()
    {
        animatorController.SetBool(PetAnimationType.Walk, true);
        randomTargetPosition = new Vector3(Random.Range(-3f, 3), Random.Range(-3f, 3f), 0);
    }

    public override void OnStateExit()
    {
        animatorController.SetBool(PetAnimationType.Walk, false);
    }

    public override void Tick()
    {
        if (player == null) return;
        if (currentTime <= 0)
        {
            randomTargetPosition = new Vector3(Random.Range(-3f, 3), Random.Range(-3f, 3f), 0);
            currentTime = 4;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        var dir = ((player.position + randomTargetPosition) - pet.position).normalized;
        pet.position += dir * (paramSystem.FollowSpeed * Time.deltaTime);
        UpdateFacing(dir);
    }

    private void UpdateFacing(Vector3 direction)
    {
        if (spriteRenderer == null) return;
        
        // Защита от быстрого переключения
        if (Time.time - lastFlipTime < flipCooldown) return;

        if (direction.x > 0.01f && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            lastFlipTime = Time.time;
        }
        else if (direction.x < -0.01f && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            lastFlipTime = Time.time;
        }
    }
}