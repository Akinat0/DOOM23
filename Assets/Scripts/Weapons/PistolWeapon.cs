using UnityEngine;

class PistolWeapon : WeaponComponent
{
    [SerializeField] GameObject shootVfxPrefab;
    [SerializeField] int damage = 10;    
    
    public override bool CanShoot()
    {
        return AmmoCount != 0;
    }

    public override void Shoot(Vector3 origin, Vector3 direction)
    {

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