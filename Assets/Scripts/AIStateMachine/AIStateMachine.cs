using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    Dictionary<string, AIState> States { get; }

    AIState activeState;

    public AIStateMachine()
    {
        States = new Dictionary<string, AIState>();
    }

    public void AddState(string stateId, AIState state)
    {
        States.Add(stateId, state);
    }

    public void SetActiveState(string stateId)
    {
        activeState?.Disable();
        activeState = States[stateId];
        activeState.Enable();
    }
}
