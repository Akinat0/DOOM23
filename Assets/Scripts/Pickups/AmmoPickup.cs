
using System.Linq;
using UnityEngine;

public class AmmoPickup : PickupItem
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] int ammoCount;
    
    protected override void PlayerEntered(PlayerController player)
    {
        base.PlayerEntered(player);

        PlayerShoot playerShoot = player.GetComponent<PlayerShoot>();

        WeaponComponent weapon = playerShoot.Weapons.FirstOrDefault(weapon => weapon.Type == weaponType);
        
        if (weapon is AmmoWeapon ammoWeapon)
        {
            ammoWeapon.AmmoCount += ammoCount;
            GameScene.Vignette.Flare(Color.white);
            Destroy(gameObject);
        }
    }
}
