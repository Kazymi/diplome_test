using StateMachine;
using UnityEngine;

public class PetFollowState : State
{
    private readonly PetConfiguration config;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private readonly Transform player;
    private readonly SpriteRenderer spriteRenderer;
    
    private Vector3 randomTarget;
    private float targetUpdateTimer;
    private float targetUpdateInterval = 2f;

    public PetFollowState(PetConfiguration config, PetAnimatorController animatorController, Transform pet, Transform player)
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
        GenerateRandomTarget();
    }

    public override void OnStateExit()
    {
        animatorController.SetBool(PetAnimationType.Walk, false);
    }

    public override void Tick()
    {
        if (player == null) return;

        var distanceToPlayer = Vector2.Distance(pet.position, player.position);
        
        if (distanceToPlayer > config.FollowDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Ходим в случайные точки возле игрока
            targetUpdateTimer += Time.deltaTime;
            
            if (targetUpdateTimer >= targetUpdateInterval || Vector2.Distance(pet.position, randomTarget) < 0.5f)
            {
                GenerateRandomTarget();
                targetUpdateTimer = 0f;
            }
            
            MoveToRandomTarget();
        }
    }

    private void MoveTowardsPlayer()
    {
        var dir = (player.position - pet.position).normalized;
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

    private void GenerateRandomTarget()
    {
        var randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var randomDistance = Random.Range(1f, config.FollowDistance * 0.8f);
        var offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * randomDistance;
        randomTarget = player.position + offset;
    }

    private void MoveToRandomTarget()
    {
        var dir = (randomTarget - pet.position).normalized;
        pet.position += dir * (config.FollowSpeed * Time.deltaTime);
        UpdateFacing(dir);
    }
}
