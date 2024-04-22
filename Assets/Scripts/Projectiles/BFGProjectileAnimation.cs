using UnityEngine;

public class BFGProjectileAnimation : MonoBehaviour
{
    [SerializeField] ProceduralQuad proceduralQuad;
    
    float startTime;
    
    void OnEnable()
    {
        startTime = Time.time;
    }
    
    void Update()
    {
        const float speed = -5f;
        proceduralQuad.Frame = Mathf.FloorToInt((Time.time - startTime) * speed);
    }
}
