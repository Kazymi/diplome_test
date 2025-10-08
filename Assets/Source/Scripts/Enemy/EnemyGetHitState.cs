using StateMachine;

public class EnemyGetHitState : State
{
    private readonly BackflipObject _flipObject;

    public EnemyGetHitState(BackflipObject flipObject)
    {
        _flipObject = flipObject;
    }

    public override void OnStateEnter()
    {
        _flipObject.ExecuteBackflip();
    }
}