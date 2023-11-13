using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] UIAim aim;

    void Update()
    {
        DamagableComponent damagable 
            = EnemyManager.GetFirstVisibleTarget(transform, 3, Affiliation.Demon | Affiliation.Neutral, 30);

        aim.CanShoot = damagable != null;
    }
}
