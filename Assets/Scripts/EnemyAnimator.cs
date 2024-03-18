
using UnityEngine;

[RequireComponent(typeof(DemonAIController))]
public class EnemyAnimator : MonoBehaviour
{
    static readonly int SpeedId = Animator.StringToHash("Speed");
    static readonly int IsDeadId = Animator.StringToHash("IsDead");
    static readonly int IsAttackingId = Animator.StringToHash("IsAttacking");
    static readonly int IsDamagedId = Animator.StringToHash("IsDamaged");
    
    Animator animator;
    DemonAIController controller;

    public Animator Animator => animator;
    public DemonAIController Controller => controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<DemonAIController>();
    }

    void Update()
    {
        Animator.SetFloat(SpeedId, Controller.CurrentSpeed / Controller.MaxSpeed);
        Animator.SetBool(IsDeadId, controller.Damagable.IsDead);
        Animator.SetBool(IsAttackingId, controller.CurrentState == "Attack");
        Animator.SetBool(IsDamagedId, controller.CurrentState   == "Damaged");
    }
}
