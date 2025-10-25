using StateMachine;
using UnityEngine;

public class PetChaseState : State
{
    private readonly PetParamSystem paramSystem;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private Transform target;
    private readonly SpriteRenderer spriteRenderer;
    private float lastFlipTime;
    private const float flipCooldown = 0.1f;

    public Transform Target => target;
    public PetChaseState(PetParamSystem paramSystem, PetAnimatorController animatorController, Transform pet, Transform target)
    {
        this.paramSystem = paramSystem;
        this.animatorController = animatorController;
        this.pet = pet;
        this.target = target;
        this.spriteRenderer = pet.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public override void OnStateEnter()
    {
        animatorController.SetBool(PetAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        animatorController.SetBool(PetAnimationType.Walk, false);
    }

    public override void Tick()
    {
        if (target == null) return;
        var dir = (target.position - pet.position).normalized;
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
