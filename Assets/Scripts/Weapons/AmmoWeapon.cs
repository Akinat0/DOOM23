
using UnityEngine;

public abstract class AmmoWeapon : WeaponComponent
{
    [SerializeField] int ammoCount;

    public int AmmoCount
    {
        get => ammoCount;
        set => ammoCount = value;
    }
}
