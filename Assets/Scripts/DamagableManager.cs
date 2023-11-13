
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DamagableManager
{
    
    static HashSet<DamagableComponent> damagableComponents = new HashSet<DamagableComponent>();
    
    public static void RegisterEnemy(DamagableComponent damagable)
    {
        damagableComponents.Add(damagable);
    }

    public static void UnregisterEnemy(DamagableComponent damagable)
    {
        damagableComponents.Remove(damagable);
    }

    public static IEnumerable<DamagableComponent> GetDamagablesByAffiliation(Affiliation affiliation)
    {
        return damagableComponents.Where(damagable => (damagable.Affiliation & affiliation) != 0);
    }

    public static DamagableComponent GetFirstVisibleTarget(Affiliation affiliation, Transform sourceTransform, float viewAngle, float maxDist)
    {
        foreach(DamagableComponent enemy in GetDamagablesByAffiliation(affiliation))
        {
            Vector3 enemyDirection = enemy.transform.position - sourceTransform.position;
            
            if(enemyDirection.sqrMagnitude > maxDist * maxDist)
                continue;
            
            Vector3 enemyDirection2D = enemyDirection;
            enemyDirection2D.y = 0;
            enemyDirection2D = enemyDirection2D.normalized;

            float angle = Mathf.Acos(Vector3.Dot(sourceTransform.forward, enemyDirection2D)) * Mathf.Rad2Deg;

            if(angle < viewAngle / 2)
            {
                CharacterController enemyCollider = enemy.GetComponent<CharacterController>();

                Vector3 unitFrac = new Vector3(0, enemyCollider.height / 2);

                if (AimLineAttack(sourceTransform.position, enemy.transform.position)
                    || AimLineAttack(sourceTransform.position, enemy.transform.position + unitFrac * 0.9f)
                    || AimLineAttack(sourceTransform.position, enemy.transform.position - unitFrac * 0.9f))
                {
                    return enemy;
                }
            }
        }

        return null;
    }
    
    static bool AimLineAttack(Vector3 sourcePos, Vector3 targetPos)
    {
        Debug.DrawLine(sourcePos, targetPos, Color.magenta);
        
        if (Physics.Linecast(sourcePos, targetPos, out RaycastHit hit)
            && hit.collider.GetComponent<DamagableComponent>())
        {
            return true;
        }
        
        return false;
    }

}
