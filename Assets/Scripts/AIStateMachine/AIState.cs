
public abstract class AIState
{
    protected AIStateMachine StateMachine { get; }

    public AIState(AIStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public abstract void Enable();
    public abstract void Disable();
    
    protected void ChangeState(string targetState)
    {
        StateMachine.SetActiveState(targetState);
    }
}
