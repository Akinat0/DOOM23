
using System.Linq;
using UnityEngine;

public class HealPickup : PickupItem
{
    [SerializeField] int hp = 10;
    
    protected override void PlayerEntered(PlayerController player)
    {
        base.PlayerEntered(player);
        

        if (player.Damagable.Hp < player.Damagable.MaxHp)
        {
            player.Damagable.Hp += hp;
            GameScene.Vignette.Flare(Color.green);
            Destroy(gameObject);
        }
    }
}
