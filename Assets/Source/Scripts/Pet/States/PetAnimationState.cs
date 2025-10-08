using StateMachine;

public class PetAnimationState : State
{
    private readonly PetAnimationType animationType;
    private readonly PetAnimatorController animatorController;

    public PetAnimationState(PetAnimationType animationType, PetAnimatorController animatorController)
    {
        this.animationType = animationType;
        this.animatorController = animatorController;
    }

    public override void OnStateEnter()
    {
        animatorController.SetTrigger(animationType);
    }
}
