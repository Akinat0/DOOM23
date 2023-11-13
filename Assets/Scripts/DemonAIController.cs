using UnityEngine;

[RequireComponent(typeof(AISense))]
public class DemonAIController : AIController
{
    AISense sense;
    AIStateMachine stateMachine;

    void Start()
    {
        sense = GetComponent<AISense>();
        
        stateMachine = new AIStateMachine();
        stateMachine.AddState("Roaming", new RoamingAIState(this, stateMachine));
        stateMachine.SetActiveState("Roaming");
    }

}
