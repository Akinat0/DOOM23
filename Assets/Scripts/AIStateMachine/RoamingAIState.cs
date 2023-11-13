
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
        AIController.Sense.TargetChanged += HandleAiTargetChanged;
    }

    public override void Disable()
    {
        AIController.AbortMoveTo();
        AIController.Sense.TargetChanged -= HandleAiTargetChanged;
    }

    void HandleMoveToCompleted(MoveToResult result)
    {
        if(result == MoveToResult.Completed)
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

    void HandleAiTargetChanged(DamagableComponent newTarget)
    {
        if (newTarget != null)
        {
            AIController.AbortMoveTo();
            ChangeState("Chasing");
        }
    }
}
