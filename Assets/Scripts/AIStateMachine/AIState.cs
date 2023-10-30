using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState 
{
    AIStateMachine StateMachine { get; }

    public AIState(AIStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public abstract void Enable();
    public abstract void Disable();

    protected void ChangeState(string stateId)
    {
        StateMachine.SetActiveState(stateId);
    }
}
