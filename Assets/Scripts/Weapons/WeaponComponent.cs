using UnityEngine;

public abstract class WeaponComponent : MonoBehaviour
{
    BaseCharacterController owner;

    public BaseCharacterController Owner => owner;

    void Awake()
    {
        owner = GetComponent<BaseCharacterController>();
    }

    public abstract WeaponType Type { get; }

    public abstract bool CanAttack();
    public abstract bool CanAttackTarget(DamagableComponent target);

    public abstract void Attack(Vector3 origin, Vector3 direction);
}