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

    public void SetActiveState(string targetStateId)
    {
        if (States.TryGetValue(targetStateId, out AIState targetState))
        {
            ActiveState?.Disable();

            ActiveState = targetState;
        
            ActiveState.Enable();
        }
        else
        {
            Debug.LogError($"State {targetStateId} is not presented in states list");
        }
       
    }
}
