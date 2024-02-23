
using System;
using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public class AISense : MonoBehaviour
{
    [SerializeField] float viewCone = 60;
    [SerializeField] float viewDist = 10;
    [SerializeField] Affiliation searchTargets = Affiliation.Any;

    public event Action<DamagableComponent> TargetChanged;

    DamagableComponent target;
    public DamagableComponent Target
    {
        get => target;
        private set
        {
            if (target != value)
            {
                target = value;
                TargetChanged?.Invoke(target);
                return;
            }
            
            target = value;
        }
    }

    DamagableComponent SelfDamagable { get; set; }

    void Awake()
    {
        SelfDamagable = GetComponent<DamagableComponent>();
    }

    void OnEnable()
    {
        SelfDamagable.HpChangedFromCharacter += HandleHpChanged;
    }

    void OnDisable()
    {
        SelfDamagable.HpChangedFromCharacter -= HandleHpChanged;
    }

    void HandleHpChanged(int delta, BaseCharacterController character)
    {
        //If we've got damage from our search target, let's set it as target
        if ((character.Damagable.Affiliation & searchTargets) != 0)
            Target = character.Damagable;
    }

    void Update()
    {
        //Probably we should not update target if we already have a valid target
        //Also we need an opportunity to loose target at radius  
        Target = DamagableManager.GetFirstVisibleTarget(searchTargets, transform, viewCone, viewDist);
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {

        Color handlesColor = UnityEditor.Handles.color;
        
        UnityEditor.Handles.color = new Color(0.6f, 0, 0, 0.1f);
        
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-45, transform.up) * transform.forward, viewCone, viewDist);

        UnityEditor.Handles.color = handlesColor;


        Color gizmosColor = Gizmos.color;
        Gizmos.color = new Color(0.6f, 0, 0, 0.1f);
        
        if(Target)
            Gizmos.DrawWireSphere(Target.transform.position, 2);

        Gizmos.color = gizmosColor;
    }
    
#endif
}
