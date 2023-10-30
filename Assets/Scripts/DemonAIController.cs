
using UnityEngine;

[RequireComponent(typeof(AISense))]
public class DemonAIController : AIController
{
    AISense sense;

    void Start()
    {
        Roam(MoveToResult.None);
        sense = GetComponent<AISense>();
        sense.TargetChanged += HandleTargetChanged;
    }

    void Roam(MoveToResult reason)
    {
        if(reason == MoveToResult.Failed)
            return;
        
        MoveTo(GetRandomReachablePosInRadius(10), Roam);
    }

    void HandleTargetChanged(DamagableComponent target)
    {
        if(target != null)
            MoveTo(target.transform.position, (_) => print("AAHAHAHHA"));
    }

    protected override void Update()
    {
        base.Update();
        
        // if (Input.GetKeyDown(KeyCode.E))
        //     MoveTo(FindObjectOfType<PlayerController>().transform.position, _ => );
    }
}
