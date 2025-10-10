using StateMachine;
using UnityEngine;

public class PetFollowState : State
{
    private readonly PetConfiguration config;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private readonly Transform player;
    private readonly SpriteRenderer spriteRenderer;

    private float targetUpdateTimer;
    private float targetUpdateInterval = 2f;
    private Vector3 randomTargetPosition;
    private float currentTime;

    public PetFollowState(PetConfiguration config, PetAnimatorController animatorController, Transform pet,
        Transform player)
    {
        this.config = config;
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