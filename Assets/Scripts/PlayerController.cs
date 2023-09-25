using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float forwardForce = 1;
    [SerializeField] float sideForce = 1;
    [SerializeField] float sensitivity = 1;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float xMouseMovement = Input.GetAxis("Mouse X");
        float horizontal = Input.GetAxis("Horizontal");           
        float vertical = Input.GetAxis("Vertical");

        Vector3 force = Vector3.zero;
        force += transform.forward * vertical * Time.fixedDeltaTime * forwardForce;
        force += transform.right * horizontal * Time.fixedDeltaTime * sideForce;

        float rotation = xMouseMovement * sensitivity * Time.fixedDeltaTime;

        rb.AddForce(force);
        transform.Rotate(0, rotation, 0);
    }
}
