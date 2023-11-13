using UnityEngine;
using UnityEngine.AI;

public class RoamingAIState : AIState
{
    public AIController AIController { get; }

    public RoamingAIState(AIController aIController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aIController;
    }

    public override void Enable()
    {
        AIController.MoveTo(GetRandomPosInRadius(10), HandleMoveToCompleted);
    }

    public override void Disable()
    {
        
    }

    void HandleMoveToCompleted(MoveToCompletedReason reason)
    {
        Debug.Log(reason);

        if (reason == MoveToCompletedReason.Failure)
            return;

        ChangeState("Roaming");
    }
    

    Vector3 GetRandomPosInRadius(float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        Vector3 targetPos = AIController.transform.position + randomDir;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, radius, NavMesh.AllAreas))
            return hit.position;
        else 
            return AIController.transform.position;
    }
}
