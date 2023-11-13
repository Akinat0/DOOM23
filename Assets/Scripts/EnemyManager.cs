
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyManager
{

    static HashSet<DamagableComponent> damagableComponents = new HashSet<DamagableComponent>();

    public static IReadOnlyCollection<DamagableComponent> Enemies => damagableComponents;

    public static void RegisterEnemy(DamagableComponent damagable)
    {
        damagableComponents.Add(damagable);
    }

    public static void UnregisterEnemy(DamagableComponent damagable)
    {
        damagableComponents.Remove(damagable);
    }

    public static DamagableComponent GetFirstVisibleTarget(
        Transform sourceTransform, 
        float coneAngle, 
        Affiliation affiliation, 
        float maxDistance) 
    { 
        
        foreach(DamagableComponent enemy in 
            EnemyManager.Enemies.Where(damagable => (damagable.Affiliation & affiliation) > 0))
        {
            Vector3 enemyDirection = enemy.transform.position - sourceTransform.position;

            if (enemyDirection.sqrMagnitude > maxDistance * maxDistance)
                continue;

            Vector3 enemyDirection2D = enemyDirection;
            enemyDirection2D.y = 0;
            enemyDirection2D = enemyDirection2D.normalized;

            enemyDirection = enemyDirection.normalized;

            float angle = Mathf.Acos(Vector3.Dot(sourceTransform.forward, enemyDirection2D)) * Mathf.Rad2Deg;

            if(angle < coneAngle)
            {
                CharacterController enemyCollider = enemy.GetComponent<CharacterController>();

                Vector3 unitFrac = new Vector3(0, enemyCollider.height / 2);

                if (AimLineAttack(sourceTransform, enemy.transform.position)
                    || AimLineAttack(sourceTransform, enemy.transform.position + unitFrac)
                    || AimLineAttack(sourceTransform, enemy.transform.position - unitFrac))
                {
                    
                    return enemy;
                }
            }
        }

        return null;
    }

    static bool AimLineAttack(Transform sourceTransform, Vector3 targetPos)
    {
        if (Physics.Linecast(sourceTransform.position, targetPos, out RaycastHit hit)
                    && hit.collider.GetComponent<DamagableComponent>())
        {
            Debug.DrawLine(sourceTransform.position, targetPos, Color.green);
            return true;
        }

        return false;
    }

}
