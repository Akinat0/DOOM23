using UnityEngine;


public class MeleeWeapon : WeaponComponent
{
    [SerializeField] int   damage = 10;
    [SerializeField] float angle = 60;
    [SerializeField] float radius = 1.5f;
    [SerializeField] float cooldown = 0.5f;

    Collider[] hitBuffer = new Collider[8];

    public override WeaponType Type => WeaponType.Melee;
    
    float lastAttackTime = float.MinValue;
    
    public override bool CanAttack()
    {
        return cooldown <= 0 || Time.time - lastAttackTime > cooldown;
    }

    public override bool CanAttackTarget(DamagableComponent target)
    {
        if (CanAttack() && target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > radius * radius)
                return false;
            
            Vector3 enemyDirection2D = target.transform.position - transform.position;
            enemyDirection2D.y = 0;
            enemyDirection2D = enemyDirection2D.normalized;

            float dot = Vector3.Dot(transform.forward, enemyDirection2D);

            //For some reason Vector3.Dot can return value outside of [-1, 1] range. It fails Acos
            dot = Mathf.Clamp(dot, -1, 1);

            if (Mathf.Acos(dot) * Mathf.Rad2Deg <= angle / 2)
                return true;
        }
    

        return false;
    }

    public override void Attack(Vector3 origin, Vector3 direction)
    {
        lastAttackTime = Time.time;
        
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, hitBuffer);

        for (int i = 0; i < count; i++)
        {
            Collider coll = hitBuffer[i];
            
            if(coll.gameObject == gameObject)
                continue;

            if (coll.TryGetComponent(out DamagableComponent damagable))
            {
                Vector3 enemyDirection2D = damagable.transform.position - transform.position;
                enemyDirection2D.y = 0;
                enemyDirection2D = enemyDirection2D.normalized;

                float dot = Vector3.Dot(transform.forward, enemyDirection2D);

                //For some reason Vector3.Dot can return value outside of [-1, 1] range. It fails Acos
                dot = Mathf.Clamp(dot, -1, 1);

                if (Mathf.Acos(dot) * Mathf.Rad2Deg <= angle / 2)
                    damagable.ApplyDamage(damage, Owner);
            }
        }
    }
}
