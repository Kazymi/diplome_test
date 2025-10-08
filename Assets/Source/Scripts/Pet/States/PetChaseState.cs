using StateMachine;
using UnityEngine;

public class PetChaseState : State
{
    private readonly PetConfiguration config;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private Transform target;
    private readonly SpriteRenderer spriteRenderer;

    public Transform Target => target;
    public PetChaseState(PetConfiguration config, PetAnimatorController animatorController, Transform pet, Transform target)
    {
        this.config = config;
        this.animatorController = animatorController;
        this.pet = pet;
        this.target = target;
        this.spriteRenderer = pet.GetComponent<SpriteRenderer>();
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
        pet.position += dir * (config.FollowSpeed * Time.deltaTime);
        UpdateFacing(dir);
    }

    private void UpdateFacing(Vector3 direction)
    {
        if (spriteRenderer == null) return;
        
        if (direction.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (direction.x < -0.01f)
            spriteRenderer.flipX = true;
    }
}
