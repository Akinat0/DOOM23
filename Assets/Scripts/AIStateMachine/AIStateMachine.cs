using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    Dictionary<string, AIState> States { get; } = new Dictionary<string, AIState>();

    public AIState ActiveState { get; private set; }
    public string ActiveStateName { get; private set; }


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
            ActiveStateName = targetStateId;
        
            ActiveState.Enable();
        }
        else
        {
            Debug.LogError($"State {targetStateId} is not presented in states list");
        }
       
    }
}
