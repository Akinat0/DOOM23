
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class EnemyAnimator : MonoBehaviour
{
    static readonly int SpeedId = Animator.StringToHash("Speed");
    static readonly int IsDeadId = Animator.StringToHash("IsDead");
    
    Animator animator;
    AIController controller;

    public Animator Animator => animator;
    public AIController Controller => controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<AIController>();
    }

    void Update()
    {
        Animator.SetFloat(SpeedId, Controller.CurrentSpeed / Controller.MaxSpeed);
        Animator.SetBool(IsDeadId, controller.Damagable.IsDead);
    }
}
