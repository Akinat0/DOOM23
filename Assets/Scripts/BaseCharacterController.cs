
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{
    [SerializeField] float speed = 10;

    GameObject floorObject;
    
    Vector3 surfaceNormal;

    CharacterController characterController;
    float verticalSpeed;
    
    public Vector3 ExternalVelocity { get; set; }
    bool IsGrounded => characterController.isGrounded 
                       || !IsOverlapRecoveryEnabled;

    public bool IsOverlapRecoveryEnabled
    {
        get => characterController.enableOverlapRecovery;
        set => characterController.enableOverlapRecovery = value;
    }
    
    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    protected void Rotate(float rotation)
    {
        transform.Rotate(new Vector3(0, rotation));
    }

    protected void SetRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, rotation));
    }

    protected void MoveWorld(float x, float z)
    {
        Vector3 direction = transform.InverseTransformDirection(new Vector3(x, 0, z));
        
        Move(direction.x, direction.z);
    }
    
    protected void Move(float forward, float right)
    {
        if (IsGrounded)
            verticalSpeed = IsOverlapRecoveryEnabled ? -0.01f : 0;
        else
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        
        Vector3 movementDirection = new Vector3(forward, 0, right);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1);

        Vector3 velocity = transform.TransformDirection(movementDirection) * speed;

        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Vector3 adjustedVelocity = slopeRotation * velocity;

        velocity = adjustedVelocity.y < 0 ? adjustedVelocity : velocity;
        velocity += ExternalVelocity;
        velocity.y += verticalSpeed;
        
        Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);

        characterController.Move(velocity * Time.deltaTime);

    }
    
    void LateUpdate()
    {
        DetectPlayerCollision(); 
    }

    void DetectPlayerCollision()
    {
        if (Physics.Linecast(transform.position, transform.position + Vector3.down * (characterController.height / 2 + 0.1f), out RaycastHit hit))
        {
            if (hit.collider.gameObject != floorObject)
            {
                if(floorObject != null)
                    floorObject.SendMessage("OnPlayerCollisionEnd", this, SendMessageOptions.DontRequireReceiver);
                
                floorObject = hit.collider.gameObject;
            }
            
            floorObject.SendMessage("OnPlayerCollisionEnter", this, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if(floorObject != null)
                floorObject.SendMessage("OnPlayerCollisionEnd", this, SendMessageOptions.DontRequireReceiver);
            
            floorObject = null;
        }
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        surfaceNormal = hit.normal;
    }
}
