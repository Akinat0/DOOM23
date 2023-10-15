using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float sensitivity = 10;

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

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 rotation = new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime);
        transform.Rotate(rotation);

        if (IsGrounded)
            verticalSpeed = IsOverlapRecoveryEnabled ? -0.01f : 0;
        else
            verticalSpeed += Physics.gravity.y * Time.deltaTime;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1);

        Vector3 velocity = transform.TransformDirection(input) * speed;

        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Vector3 adjustedVelocity = slopeRotation * velocity;

        velocity = adjustedVelocity.y < 0 ? adjustedVelocity : velocity;
        velocity += ExternalVelocity;
        velocity.y += verticalSpeed;

        characterController.Move(velocity * Time.deltaTime);
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        surfaceNormal = hit.normal;
    }
}