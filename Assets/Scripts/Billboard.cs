using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cachedCamera;

    void Start()
    {
        cachedCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(new Vector3(cachedCamera.position.x, transform.position.y, cachedCamera.position.z));
    }
}
