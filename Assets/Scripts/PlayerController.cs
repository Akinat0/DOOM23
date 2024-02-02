using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : BaseCharacterController
{
    [SerializeField] float sensitivity = 10;
    [SerializeField] Camera playerCamera;
    
    Vector3 surfaceNormal;

    CharacterController characterController;
    float verticalSpeed;

    public override float Height => characterController.height;
    public override float Radius => characterController.radius;
    
    bool IsGrounded => characterController.isGrounded;
    
    protected void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 30;
    }

    void Update()
    {

        Rotate(Input.GetAxisRaw("Mouse X") * sensitivity);
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        Transform cameraTransform = playerCamera.transform;
        Vector3 cameraRotation = cameraTransform.localEulerAngles;
        
        if (cameraRotation.x > 180)
            cameraRotation.x -= 360;
        
        cameraRotation.x = Mathf.Clamp(cameraRotation.x - Input.GetAxisRaw("Mouse Y") * sensitivity, -60, 60);
        cameraTransform.localEulerAngles = cameraRotation;
    }
    

    protected void Rotate(float rotation)
    {
        transform.Rotate(new Vector3(0, rotation));
    }

    protected void Move(float forward, float right)
    {
        if (IsGrounded)
            verticalSpeed = -0.01f;
        else
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        
        Vector3 movementDirection = new Vector3(forward, 0, right);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1);

        Vector3 velocity = transform.TransformDirection(movementDirection) * MaxSpeed;

        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Vector3 adjustedVelocity = slopeRotation * velocity;

        velocity = adjustedVelocity.y < 0 ? adjustedVelocity : velocity;
        velocity.y += verticalSpeed;
        
        if(surfaceNormal != Vector3.up)
            Debug.DrawLine(transform.position, transform.position + surfaceNormal, Color.blue, 10);

        characterController.Move(velocity * Time.deltaTime);

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        surfaceNormal = hit.normal;
    }
}