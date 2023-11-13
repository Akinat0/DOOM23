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

public class AIController : BaseCharacterController
{
    bool isMoveToCompleted = true;
    NavMeshPath path;
    int targetPointIndex;

    Vector3 moveToTargetPos;
    Action<MoveToResult> moveToCompleted;

    protected override void Awake()
    {
        base.Awake();

        path = new NavMeshPath();
    }

    public bool MoveTo(Vector3 targetPos, Action<MoveToResult> completed = null, bool allowPartial = false)
    {
        //previous task is not completed
        if (!isMoveToCompleted)
            InvokeMoveToCompleted(MoveToResult.Aborted);
        
        moveToCompleted = completed;
        moveToTargetPos = targetPos;
        targetPointIndex = 1;
        isMoveToCompleted = false;
        
        NavMesh.CalculatePath(transform.position, moveToTargetPos, NavMesh.AllAreas, path);

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

        if (Vector3.Distance(targetPos, sourcePos) < 1)
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
