using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float rotationSpeed = 10;
    
    CharacterController controller;

    Vector3 surfaceNormal;
    float ySpeed;

    public Vector3 ExternalVelocity { get; set; }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        Vector3 rotation = new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(rotation);
        
        if (controller.isGrounded)
            ySpeed = -0.1f;
        else
            ySpeed += Physics.gravity.y * Time.deltaTime;
        
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1);
        Vector3 velocity = transform.TransformDirection(input) * moveSpeed;
        
        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Vector3 surfaceAdjustedVelocity = slopeRotation * velocity;
        
        velocity = surfaceAdjustedVelocity.y < 0 ? surfaceAdjustedVelocity : velocity;
        velocity += ExternalVelocity;
        velocity.y += ySpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        surfaceNormal = hit.normal;
    }
}

