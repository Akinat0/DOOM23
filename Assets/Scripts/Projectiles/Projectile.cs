using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    [SerializeField] float timeout = 5;
    
    public event Action<Projectile, Collision> CollisionHandler;
    
    Rigidbody Rigidbody { get; set; }
    float startTime; 

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > timeout)
            CollisionHandler?.Invoke(this, null);
    }

    void OnCollisionEnter(Collision other)
    {
        CollisionHandler?.Invoke(this, other);
    }

    void FixedUpdate()
    {
        Rigidbody.MovePosition(transform.position + transform.forward * speed);
    }
}
