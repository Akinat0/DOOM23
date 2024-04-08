using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PickupItem : MonoBehaviour
{
    
    Collider Collider;
    
    void Awake()
    {
        Collider = GetComponent<SphereCollider>();
        Collider.isTrigger = true;
    }

    Vector3 cachedPos;
    
    void Start()
    {
        cachedPos = transform.position;
    }
    
    void Update()
    {
        transform.position = cachedPos + Vector3.up * (0.25f * Mathf.Sin(Time.time * 2));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
            PlayerEntered(player);
        
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
            PlayerExited(player);
    }

    protected virtual void PlayerEntered(PlayerController player)
    {
        
    }
    
    protected virtual void PlayerExited(PlayerController player)
    {
        
    }


    
}
