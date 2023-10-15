using System;
using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float height = 5;
    [SerializeField] float speed = 5;
    [SerializeField] float delay = 2;
    [SerializeField] Transform elevatorTransform;

    IEnumerator elevateRoutine;

    bool isActive;
    
    void OnTriggerEnter(Collider other)
    {
        print("Trigger elevator");
        if (other.TryGetComponent(out PlayerController playerController) && elevateRoutine == null)
        {
            // isActive = true;
            StartCoroutine(elevateRoutine = ElevateRoutine(playerController));
        }
    }

    IEnumerator ElevateRoutine(PlayerController playerController)
    {
        float duration = height / speed;
        float time = 0;
    
        playerController.ExternalVelocity += Vector3.up * speed;
        playerController.GetComponent<CharacterController>().enableOverlapRecovery = false;
    
        while (time < duration)
        {
            time += Time.deltaTime;
            elevatorTransform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, height), time / duration);
            yield return null;
        }
    
        playerController.GetComponent<CharacterController>().enableOverlapRecovery = true;
        playerController.ExternalVelocity -= Vector3.up * speed;
        
        
        elevatorTransform.localPosition = new Vector3(0, height);
    
        yield return new WaitForSeconds(delay);
    
        time = 0;
    
        playerController.ExternalVelocity += Vector3.down * speed;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            elevatorTransform.localPosition = Vector3.Lerp(new Vector3(0, height), Vector3.zero, time / duration);
            yield return null;
        }
        
        playerController.ExternalVelocity -= Vector3.down * speed;
    
        elevatorTransform.localPosition = Vector3.zero;
    
        elevateRoutine = null;
    }
    
}
