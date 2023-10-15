using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] UIAim aim;
    
    void Update()
    {
        foreach(DamagableComponent enemy in EnemyManager.Enemies)
        {
            Vector3 enemyDirection = enemy.transform.position - transform.position;
            
            Vector3 enemyDirection2D = enemyDirection;
            enemyDirection2D.y = 0;
            enemyDirection2D = enemyDirection2D.normalized;

            enemyDirection = enemyDirection.normalized;

            float angle = Mathf.Acos(Vector3.Dot(transform.forward, enemyDirection2D)) * Mathf.Rad2Deg;

            if(angle < 3)
            {
                CapsuleCollider enemyCollider = enemy.GetComponent<CapsuleCollider>();

                Vector3 unitFrac = new Vector3(0, enemyCollider.height / 2);

                if (AimLineAttack(enemy.transform.position)
                    || AimLineAttack(enemy.transform.position + unitFrac)
                    || AimLineAttack(enemy.transform.position - unitFrac))
                {
                    aim.CanShoot = true;
                    return;
                }
            }
        }

        aim.CanShoot = false;
    }

    bool AimLineAttack(Vector3 targetPos)
    {
        if (Physics.Linecast(transform.position, targetPos, out RaycastHit hit)
                    && hit.collider.GetComponent<DamagableComponent>())
        {
            return true;
        }

        return false;
    }
}
