using StateMachine;

public class EnemyAnimationState : State
{
    private readonly EnemyAnimationType type;
    private readonly EnemyAnimatorController animatorController;

    public EnemyAnimationState(EnemyAnimationType type, EnemyAnimatorController animatorController)
    {
        this.type = type;
        this.animatorController = animatorController;
    }

    public override void OnStateEnter()
    {
        animatorController.SetBool(type, true);
    }

    public override void OnStateExit()
    {
        animatorController.SetBool(type, false);
    }
}


