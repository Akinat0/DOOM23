using System;
using UnityEngine;
using UnityEngine.AI;

public enum MoveToResult
{
    None, 
    Completed, 
    Failed, 
    Aborted
}

[RequireComponent(typeof(AISense), typeof(NavMeshAgent))]
public class AIController : BaseCharacterController
{
    bool isMoveToCompleted = true;
    float moveToAcceptanceRadius;
    Vector3 moveToTargetPos;
    Action<MoveToResult> moveToCompleted;

    AISense sense;
    NavMeshAgent agent;

    public AISense Sense => sense;
    public override float Height => agent.height;
    public override float Radius => agent.radius;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sense = GetComponent<AISense>();

        agent.angularSpeed = 1000;
        agent.acceleration = 1000;
        agent.speed = Speed;
    }

    public bool MoveTo(Vector3 targetPos, Action<MoveToResult> completed = null, float acceptanceRadius = 1)
    {
        if (!isMoveToCompleted)
            InvokeMoveToCompleted(MoveToResult.Aborted);

        isMoveToCompleted = false;
        moveToTargetPos = targetPos;
        moveToCompleted = completed;
        moveToAcceptanceRadius = acceptanceRadius;

        if (!agent.SetDestination(targetPos))
        {
            InvokeMoveToCompleted(MoveToResult.Failed);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AbortMoveTo()
    {
        InvokeMoveToCompleted(MoveToResult.Aborted);
    }

    protected virtual void Update()
    {
        if(isMoveToCompleted)
            return;

        if (Vector3.SqrMagnitude(moveToTargetPos - transform.position) 
            <= moveToAcceptanceRadius * moveToAcceptanceRadius)
        {
            InvokeMoveToCompleted(MoveToResult.Completed);
        }
    }

    void InvokeMoveToCompleted(MoveToResult reason)
    {
        isMoveToCompleted = true;
        
        Action<MoveToResult> action = moveToCompleted;
        moveToCompleted = null;
        action?.Invoke(reason);
    }
}
