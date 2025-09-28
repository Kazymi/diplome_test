using StateMachine;

public class PlayerAnimationState : State
{
    private readonly PlayerAnimationType _playerAnimationType;
    private readonly PlayerAnimatorController _playerAnimatorController;

    public PlayerAnimationState(PlayerAnimationType playerAnimationType,
        PlayerAnimatorController playerAnimatorController)
    {
        _playerAnimationType = playerAnimationType;
        _playerAnimatorController = playerAnimatorController;
    }

    public override void OnStateEnter()
    {
        _playerAnimatorController.SetBool(_playerAnimationType, true);
    }

    public override void OnStateExit()
    {
        _playerAnimatorController.SetBool(_playerAnimationType, false);
    }
}