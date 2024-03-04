
using UnityEngine;
using UnityEngine.AI;

public class DeadAIState : AIState
{
    public AIController AIController { get; }

    public DeadAIState(AIController aiController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aiController;
    }

    public override void Enable()
    {
        AIController.Sense.enabled = false;
        AIController.GetComponent<Collider>().enabled = false;
        AIController.GetComponent<NavMeshAgent>().enabled = false;
    }

    public override void Disable()
    {
        AIController.Sense.enabled = true;
        AIController.GetComponent<Collider>().enabled = true;
        AIController.GetComponent<NavMeshAgent>().enabled = true;
    }
}
