using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cachedCameraTransform;

    void Awake()
    {
        cachedCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector3 targetPos = cachedCameraTransform.position;
        targetPos.y = transform.position.y;
        
        transform.LookAt(targetPos);
    }
}
