using UnityEngine;
using UnityEngine.Rendering;

class PistolWeapon : AmmoWeapon
{
    [SerializeField] int   damage = 10;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float radius = 10;
    [SerializeField] float angle = 1;
    
    [SerializeField] int   angleVariance = 1;

    public float Cooldown => cooldown;
    
    public float Radius
    {
        get => radius;
        set => radius = value;
    }
    
    public float Angle
    {
        get => angle;
        set => angle = value;
    }
    
    float lastShootTime = float.MinValue;

    public override WeaponType Type => WeaponType.Pistol;

    public override bool CanAttack()
    {
        return (AmmoCount != 0 && (Cooldown <= 0 || Time.time - lastShootTime > Cooldown));
    }
    
    public override bool CanAttackTarget(DamagableComponent target)
    {
        if (CanAttack() && target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            if (direction.sqrMagnitude > Radius * Radius)
                return false;

            //2D direction
            direction.y = 0;
            
            float dot = Vector3.Dot(direction.normalized, transform.forward);
            
            //For some reason Vector3.Dot can return value outside of [-1, 1] range. It fails Acos.
            dot = Mathf.Clamp(dot, -1, 1);
            
            float acos = Mathf.Acos(dot);
            float degree = acos * Mathf.Rad2Deg;
            
            return Mathf.Abs(degree) <= Angle;
        }

        return false;
    }

    public override void Attack(Vector3 origin, Vector3 direction)
    {
        Quaternion variance = 
            Quaternion.Euler(
                (0.5f - Random.value) * angleVariance, 
                (0.5f - Random.value) * angleVariance,
                (0.5f - Random.value) * angleVariance);
        
        direction = variance * direction;
        lastShootTime = Time.time;
        Debug.DrawLine(origin, origin + direction * 100, Color.blue, 10);

        //we didn't hit anything
        if (!PhysicsUtility.RaycastIgnoreSelf(origin, direction, transform, out RaycastHit hit))
            return;

        if (!hit.collider.TryGetComponent(out DamagableComponent damagable))
        {
            //we've hit wall or something solid
            GameObject vfx = GameScene.PuffFactory.Get();
            vfx.transform.position = hit.point;
            vfx.transform.rotation = Quaternion.identity;
            
            return;
        }

        damagable.ApplyDamage(damage, Owner);
    }
    
#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Color handlesColor = UnityEditor.Handles.color;
        CompareFunction handlesZTest = UnityEditor.Handles.zTest;
        
        UnityEditor.Handles.color = new Color(0.0f, 1, 0, 0.1f);
        UnityEditor.Handles.zTest = CompareFunction.LessEqual;

        //Draw touch radius
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, Radius);
        
        UnityEditor.Handles.color = handlesColor;
        UnityEditor.Handles.zTest = handlesZTest;
        
    }
#endif
}