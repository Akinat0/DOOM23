
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));

            yield return new WaitUntil(CanAttack);
            
            Attack();
        }
    }

    bool CanAttack()
    {
        return Physics.Raycast(transform.position, transform.forward, out RaycastHit hit) 
               && hit.collider.GetComponent<PlayerController>();
    }
    
    void Attack()
    {
        
    }
}
