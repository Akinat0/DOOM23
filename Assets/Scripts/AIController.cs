using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum MoveToResult
{
    None, 
    Completed, 
    Failed, 
    Aborted
}

[RequireComponent(typeof(AISense))]
public class AIController : BaseCharacterController
{
    bool isMoveToCompleted = true;
    NavMeshPath path;
    int targetPointIndex;

    float moveToAcceptanceRadius;
    Vector3 moveToTargetPos;
    Action<MoveToResult> moveToCompleted;

    AISense sense;

    public AISense Sense => sense;

    protected override void Awake()
    {
        base.Awake();

        path = new NavMeshPath();
        sense = GetComponent<AISense>();
    }

    public bool MoveTo(Vector3 targetPos, Action<MoveToResult> completed = null, bool allowPartial = false, float acceptanceRadius = 3)
    {
        //previous task is not completed
        if (!isMoveToCompleted)
            InvokeMoveToCompleted(MoveToResult.Aborted);
        
        moveToCompleted = completed;
        targetPointIndex = 1;
        isMoveToCompleted = false;
        moveToAcceptanceRadius = acceptanceRadius;

        if (!NavMesh.SamplePosition(targetPos, out NavMeshHit hit, acceptanceRadius, NavMesh.AllAreas))
        {
            InvokeMoveToCompleted(MoveToResult.Failed);
            return false;
        }

        moveToTargetPos = hit.position;
        
        NavMesh.CalculatePath(transform.position, moveToTargetPos, NavMesh.AllAreas, path);

        if (path.corners.Length == 1)
        {
            InvokeMoveToCompleted(MoveToResult.Completed);
            return true;
        }

        bool hasPath = path.status != NavMeshPathStatus.PathInvalid 
                       && (allowPartial || path.status == NavMeshPathStatus.PathComplete); 
        
        if (!hasPath)
            InvokeMoveToCompleted(MoveToResult.Failed);

        return hasPath;
    }

    public void AbortMoveTo()
    {
        InvokeMoveToCompleted(MoveToResult.Aborted);
    }

    protected virtual void Update()
    {
        UpdateMovement();
    }


    void UpdateMovement()
    {
        if(isMoveToCompleted)
            return;

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        
        Vector3 targetPos = path.corners[targetPointIndex];
        Vector3 sourcePos = transform.position;

        //Remove vertical
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        sourcePos = new Vector3(sourcePos.x, 0, sourcePos.z);

        float tolerance = targetPointIndex == path.corners.Length - 1 ? moveToAcceptanceRadius : 1;
        
        if (Vector3.Distance(targetPos, sourcePos) < tolerance)
        {
            if (targetPointIndex + 1 >= path.corners.Length)
            {
                InvokeMoveToCompleted(MoveToResult.Completed);
                return;
            }
            
            targetPointIndex++;
            targetPos = path.corners[targetPointIndex];
        }

        Vector3 direction = (targetPos - sourcePos).normalized; 
        
        SetRotation(Quaternion.LookRotation(direction).eulerAngles.y);
        MoveWorld(direction.x, direction.z);
    }

    void InvokeMoveToCompleted(MoveToResult reason)
    {
        isMoveToCompleted = true;
        
        Action<MoveToResult> action = moveToCompleted;
        moveToCompleted = null;
        action?.Invoke(reason);
    }
    
}
