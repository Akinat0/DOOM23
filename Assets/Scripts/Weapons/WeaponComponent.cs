using UnityEngine;

public abstract class WeaponComponent : MonoBehaviour
{
    BaseCharacterController owner;

    public BaseCharacterController Owner => owner;

    void Awake()
    {
        owner = GetComponent<BaseCharacterController>();
    }

    public abstract bool CanAttack(DamagableComponent target = null);

    public abstract void Attack(Vector3 origin, Vector3 direction);
}