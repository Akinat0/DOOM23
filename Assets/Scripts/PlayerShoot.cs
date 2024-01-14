using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] UIAim aim;
    
    void Update()
    {
        DamagableComponent target = DamagableManager.GetFirstVisibleTarget(Affiliation.Demons | Affiliation.Neutral, transform, 360, 100);
        aim.CanShoot = target != null;

        if (Input.GetMouseButtonDown(0) && target != null)
        {
            target.Hp -= 10;
            print($"Shoot! Hp remains {target.Hp}. Is Dead: {target.IsDead}");
        }
    }
}
