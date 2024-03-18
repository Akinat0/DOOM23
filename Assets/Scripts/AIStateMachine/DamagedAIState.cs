
public class DamagedAIState : AIState
{
    AIController Controller { get; }

    public DamagedAIState(AIController controller, AIStateMachine stateMachine) : base(stateMachine)
    {
        Controller = controller;
    }

    public override void Enable()
    {
        Controller.AnimationNotified += HandleAnimationNotified;
    }

    public override void Disable()
    {
        Controller.AnimationNotified -= HandleAnimationNotified;   
    }

    void HandleAnimationNotified(string notification)
    {
        if (notification == "damaged_completed")
            ChangeState("Chasing");
    }
}
