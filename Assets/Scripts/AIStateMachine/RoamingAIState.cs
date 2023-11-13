
using UnityEngine;
using UnityEngine.AI;

public class RoamingAIState : AIState
{
    AIController AIController { get; }


    public RoamingAIState(AIController aiController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aiController;
    }
    
    public override void Enable()
    {
        AIController.MoveTo(GetRandomPosInRadius(10), HandleMoveToCompleted);
    }

    public override void Disable()
    {
        AIController.AbortMoveTo();
    }

    void HandleMoveToCompleted(MoveToResult result)
    {
        ChangeState("Roaming");
    }
    
    protected Vector3 GetRandomPosInRadius(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += AIController.transform.position;
        
        return NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas) 
            ? hit.position 
            : AIController.transform.position;
    }
}
