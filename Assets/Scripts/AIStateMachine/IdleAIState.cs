
public class IdleAIState : AIState
{
    AIController AIController { get; }
 
    public IdleAIState(AIController aiController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aiController;
    }

    public override void Enable()
    {
        AIController.Sense.TargetChanged += HandleAiTargetChanged;
    }

    public override void Disable()
    {
        AIController.Sense.TargetChanged -= HandleAiTargetChanged;

    }
    
    void HandleAiTargetChanged(DamagableComponent newTarget)
    {
        if (newTarget != null)
            ChangeState("Chasing");
        
    }
}
