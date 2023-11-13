using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    Dictionary<string, AIState> States { get; } = new Dictionary<string, AIState>();

    AIState ActiveState { get; set; }


    public void AddState(string stateId, AIState state)
    {
        States.Add(stateId, state);
    }

    public void SetActiveState(string targetState)
    {
        ActiveState?.Disable();

        ActiveState = States[targetState];
        
        Debug.Log("Change state to " + targetState);
        
        ActiveState.Enable();
    }
}
