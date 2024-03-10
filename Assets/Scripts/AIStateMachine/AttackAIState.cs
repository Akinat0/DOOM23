using System;
using UnityEngine;

public class AttackAIState : AIState
{
    public AIController AIController { get; }

    public AttackAIState(AIController aiController, AIStateMachine stateMachine) : base(stateMachine)
    {
        AIController = aiController;
    }

    Vector3 cachedTargetPos;

    public override void Enable()
    {
        AIController.AnimationNotified += HandleAnimationNotified;
        cachedTargetPos = AIController.Sense.Target.transform.position;
    }

    public override void Disable()
    {
        AIController.AnimationNotified -= HandleAnimationNotified;
    }

    void HandleAnimationNotified(string notification)
    {
        if (string.Equals("attack", notification, StringComparison.InvariantCultureIgnoreCase))
            Attack();
        else if (string.Equals("attack_completed", notification, StringComparison.InvariantCultureIgnoreCase))
            ChangeState("Chasing");
    }
    
    void Attack()
    {
        Vector3 sourcePos = AIController.transform.position + new Vector3(0, AIController.Height / 2);
        Vector3 direction = Vector3.Normalize(cachedTargetPos - sourcePos);
        AIController.Weapon.Attack(sourcePos, direction);
    }
    
}
