
using UnityEngine;

public class DemonAIController : AIController
{
    AIStateMachine stateMachine;

    public string CurrentState => stateMachine.ActiveStateName;

    [SerializeField] WeaponType weaponType;

    void Start()
    {
        stateMachine = new AIStateMachine();
        stateMachine.AddState("Idle",    new IdleAIState(this, stateMachine));
        stateMachine.AddState("Roaming", new RoamingAIState(this, stateMachine));
        stateMachine.AddState("Chasing", new ChasingAIState(this, stateMachine));
        stateMachine.AddState("Dead",    new DeadAIState(this, stateMachine));
        stateMachine.AddState("Attack",  new AttackAIState(this, stateMachine));
        stateMachine.AddState("Damaged", new DamagedAIState(this, stateMachine));
        stateMachine.SetActiveState("Idle");

        if (Weapon == null && weaponType != WeaponType.None)
            Weapon = WeaponHelper.AddWeapon(gameObject, weaponType);
        
        
        Damagable.HpChangedFromCharacter += HandleHpChangedFromCharacter;
    }

    void OnDestroy()
    {
        Damagable.HpChangedFromCharacter -= HandleHpChangedFromCharacter;
    }

    protected override void Update()
    {
        base.Update();

        if (Damagable.IsDead && stateMachine.ActiveState is not DeadAIState)
            stateMachine.SetActiveState("Dead");
    }
    
    void HandleHpChangedFromCharacter(int damage, BaseCharacterController damager)
    {
        Vector3 direction = damager.transform.position - transform.position;
        direction.y = 0;
        direction = direction.normalized;

        //look at damager
        transform.forward = direction;
        
        if (damage > 0 && !Damagable.IsDead && stateMachine.ActiveStateName != "Attack")
        {
            stateMachine.SetActiveState("Damaged");
        }
    }

}
