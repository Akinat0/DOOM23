using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] float precision = 3;
    
    public EnemiesManager EnemiesManager { get; set; }

    void Update()
    {
        Vector3 forward = transform.forward;
        
        foreach (Enemy enemy in EnemiesManager.Enemies)
        {
            Vector3 enemyDirection = (enemy.transform.position - transform.position).normalized;
            
            float dot = Vector3.Dot(forward, enemyDirection);

            float angleRad = Mathf.Acos(dot);
            float angleDeg = angleRad * Mathf.Rad2Deg;
            
            // Debug.Log(angleDeg);
            Ray ray = new Ray(transform.position, enemyDirection);
            
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1, false);
            
            if (angleDeg >= precision)
                continue;
            
                
            if (Physics.Raycast(ray, out RaycastHit hit) 
                && hit.collider.gameObject == enemy.gameObject)
            {
                Debug.Log("Can shoot");
            }
        }
    }
    
    
}
