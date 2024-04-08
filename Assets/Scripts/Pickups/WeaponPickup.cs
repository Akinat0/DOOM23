
using UnityEngine;

public class WeaponPickup : PickupItem
{
    [SerializeField] WeaponType weaponType;
    
    protected override void PlayerEntered(PlayerController player)
    {
        base.PlayerEntered(player);

        PlayerShoot playerShoot = player.GetComponent<PlayerShoot>();

        if (playerShoot.AddWeapon(weaponType))
        {
            GameScene.Vignette.Flare(Color.white);
            Destroy(gameObject);
        }
    }
}
