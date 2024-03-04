using UnityEngine;

class PistolWeapon : WeaponComponent
{
    [SerializeField] GameObject shootVfxPrefab;
    [SerializeField] int damage = 10;
    [SerializeField] float cooldown = 1f;

    public float Cooldown => cooldown;

    float lastShootTime = float.MinValue;
    
    public override bool CanShoot()
    {
        return AmmoCount != 0 && (Cooldown <= 0 || Time.time - lastShootTime > Cooldown);
    }

    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        lastShootTime = Time.time;
        Debug.DrawLine(origin, origin + direction * 100, Color.blue, 10);

        //we didn't hit anything
        if (!PhysicsUtility.RaycastIgnoreSelf(origin, direction, transform, out RaycastHit hit))
            return;

        if (!hit.collider.TryGetComponent(out DamagableComponent damagable))
        {
            //we've hit wall or something solid
            if (shootVfxPrefab)
                Instantiate(shootVfxPrefab, hit.point, Quaternion.identity);
            return;
        }

        damagable.ApplyDamage(damage, Owner);
    }
}