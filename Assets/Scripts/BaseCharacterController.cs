using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseCharacterController : MonoBehaviour
{
    [SerializeField] float speed = 10;

    Vector3 surfaceNormal;
    CharacterController characterController;
    float verticalSpeed;
    GameObject floor;


    GameObject Floor
    {
        get => floor;
        set
        {
            if (floor != value)
            {
                if (floor != null)
                    floor.SendMessage("OnCharacterExit", this, SendMessageOptions.DontRequireReceiver);

                if (value != null)
                    value.SendMessage("OnCharacterEnter", this, SendMessageOptions.DontRequireReceiver);
            }

            floor = value;
        }
    }

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    protected void Rotate(float angle)
    {
        transform.Rotate(new Vector3(0, angle));
    }

    protected void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, angle));
    }

    protected void MoveLocal(float forward, float right)
    {
        if (characterController.isGrounded)
            verticalSpeed = -0.1f;
        else
            verticalSpeed += Physics.gravity.y * Time.deltaTime;

        Vector3 input = new Vector3(forward, 0, right);
        input = Vector3.ClampMagnitude(input, 1);

        Vector3 velocity = transform.TransformDirection(input) * speed;

        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        Vector3 adjustedVelocity = slopeRotation * velocity;

        velocity = adjustedVelocity.y < 0 ? adjustedVelocity : velocity;

        velocity.y += verticalSpeed;

        characterController.Move(velocity * Time.deltaTime);

        GroundCheck();
    }

    protected void MoveWorld(float x, float z)
    {
        Vector3 direction = transform.InverseTransformDirection(new Vector3(x, 0, z));

        MoveLocal(direction.x, direction.z);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawLine(hit.point, hit.point + hit.normal * 10, Color.red);

        surfaceNormal = hit.normal;
    }


    void GroundCheck()
    {
        if (Physics.Linecast(
            transform.position,
            transform.position + Vector3.down * (characterController.height / 2 + 0.1f),
            out RaycastHit hit))
        {
            Floor = hit.collider.gameObject;
            Floor.SendMessage("OnCharacterStay", this, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Floor = null;
        }
    }

}
