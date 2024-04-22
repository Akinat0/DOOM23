using UnityEngine;

public class BFGWeapon : AmmoWeapon
{
    [SerializeField] float cooldown = 1f;
    [SerializeField] float angle = 3f;
    [SerializeField] float radius = 25;
    
    public override WeaponType Type => WeaponType.BFG;
    
    float lastShootTime = float.MinValue;
    
    public override bool CanAttack()
    {
        return AmmoCount >= 0 && (cooldown <= 0 || Time.time - lastShootTime > cooldown);
    }

    public override bool CanAttackTarget(DamagableComponent target)
    {
        if (CanAttack() && target != null)
        {
            if (target != null)
            {
                Vector3 direction = target.transform.position - transform.position;

                if (direction.sqrMagnitude > radius * radius)
                    return false;

                //2D direction
                direction.y = 0;
                
                float dot = Vector3.Dot(direction.normalized, transform.forward);
                
                //For some reason Vector3.Dot can return value outside of [-1, 1] range. It fails Acos.
                dot = Mathf.Clamp(dot, -1, 1);
                
                float acos = Mathf.Acos(dot);
                float degree = acos * Mathf.Rad2Deg;
                
                return Mathf.Abs(degree) <= angle;

            }
            
            return true;
        }

        return false;
    }

    public override void Attack(Vector3 origin, Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);

        Projectile projectile = GameScene.BFGProjectileFactory.Get<Projectile>(origin + direction * 3, rotation);

        projectile.CollisionHandler += HandleProjectileCollision;
    }
    
    void HandleProjectileCollision(Projectile projectile, Collision collision)
    {
        if (collision != null && collision.gameObject.TryGetComponent(out DamagableComponent damagable))
            damagable.ApplyDamage(25, Owner);
        
        projectile.CollisionHandler -= HandleProjectileCollision;

        GameScene.BFGPuffFactory.Get(projectile.transform.position);
        GameScene.BFGProjectileFactory.Release(projectile);
        
    }
}
