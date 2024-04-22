
using System;
using UnityEngine;

public static class WeaponHelper
{
    public static WeaponComponent AddWeapon(GameObject gameObject, WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                return gameObject.AddComponent<PistolWeapon>();
            case WeaponType.Shotgun:
                return gameObject.AddComponent<ShotgunWeapon>();
            case WeaponType.Melee:
                return gameObject.AddComponent<MeleeWeapon>();
            case WeaponType.BFG:
                return gameObject.AddComponent<BFGWeapon>();
            default:
                throw new Exception(
                    $"Weapon type {weaponType} is not presented in WeaponHelper.AddWeapon's switch case. Please add it there.");

                
        }
    } 
}
