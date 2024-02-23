

public class DemonAIController : AIController
{
    AIStateMachine stateMachine;

    void Start()
    {
        stateMachine = new AIStateMachine();
        stateMachine.AddState("Idle", new IdleAIState(this, stateMachine));
        stateMachine.AddState("Roaming", new RoamingAIState(this, stateMachine));
        stateMachine.AddState("Chasing", new ChasingAIState(this, stateMachine));
        stateMachine.AddState("Dead", new DeadAIState(stateMachine));
        stateMachine.SetActiveState("Idle");
    }

    protected override void Update()
    {
        base.Update();

        if (Damagable.IsDead && stateMachine.ActiveState is not DeadAIState)
            stateMachine.SetActiveState("Dead");
        
    }

}
