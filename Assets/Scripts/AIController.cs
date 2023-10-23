using UnityEngine;
using UnityEngine.AI;

public class AIController : BaseCharacterController
{
    bool isMoveToCompleted = true;
    NavMeshPath path;
    int targetPointIndex;

    Vector3 moveToTargetPos;

    protected override void Awake()
    {
        base.Awake();

        path = new NavMeshPath();
    }

    public bool MoveTo(Vector3 targetPos)
    {
        moveToTargetPos = targetPos;
        
        bool hasPath = NavMesh.CalculatePath(transform.position, moveToTargetPos, NavMesh.AllAreas, path);

        if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
            targetPointIndex = 1;

        isMoveToCompleted = !hasPath;

        return hasPath;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            MoveTo(FindObjectOfType<PlayerController>().transform.position);

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
                Debug.Log("Complete!");
                isMoveToCompleted = true;
                return;
            }
            
            targetPointIndex++;
            targetPos = path.corners[targetPointIndex];
        }

        Vector3 direction = (targetPos - sourcePos).normalized; 
        
        SetRotation(Quaternion.LookRotation(direction).eulerAngles.y);
        MoveWorld(direction.x, direction.z);
    }
}
