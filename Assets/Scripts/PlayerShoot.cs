using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] UIAim aim;
    
    void Update()
    {
        DamagableComponent target = DamagableManager.GetFirstVisibleTarget(Affiliation.Demons | Affiliation.Neutral, transform, 3, 100);
        aim.CanShoot = target != null;
    }
}
