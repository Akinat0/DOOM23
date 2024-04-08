
using System.Collections;
using UnityEngine;

public class ChasingAIState : AIState
{
    public AIController AIController { get; }

    IEnumerator chaseRoutine;

    public ChasingAIState(AIController aiController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aiController;
    }

    public override void Enable()
    {
        Coroutines.StartCoroutine(chaseRoutine = ChaseRoutine());
    }

    public override void Disable()
    {
        AIController.AbortMoveTo();
        Coroutines.StopCoroutine(chaseRoutine);
    }

    IEnumerator ChaseRoutine()
    {
        Vector3 targetPos = Vector3.zero; 
        
        while (true)
        {

            if (AIController.Sense.Target != null)
            {
                targetPos = AIController.Sense.Target.transform.position;

                if (AIController.Weapon != null
                    && AIController.Weapon.CanAttackTarget(AIController.Sense.Target))
                {
                    ChangeState("Attack");
                }
                else
                {
                    AIController.MoveTo(targetPos, HandleMoveToCompleted);
                }
            }

            yield return null;
        }
    }

    void HandleMoveToCompleted(MoveToResult result)
    {
        
        if (result == MoveToResult.Completed)
        {
            ChangeState("Roaming");
        }

    }
}
