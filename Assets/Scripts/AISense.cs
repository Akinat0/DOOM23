
using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(DamagableComponent))]
public class AISense : MonoBehaviour
{
    [SerializeField] float viewCone = 60;
    [SerializeField] float viewDist = 10;
    [SerializeField] float touchRadius = 2f;
    [SerializeField] Affiliation searchTargets = Affiliation.Any;


    readonly Collider[] hitBuffer = new Collider[5];
    
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
        Target = DamagableManager.GetFirstVisibleTarget(searchTargets, transform, viewCone, viewDist, Target != null);

        if (Target != null)
            return;
        
        //If we didn't find anybody check touch sense

        int count = Physics.OverlapSphereNonAlloc(transform.position, touchRadius, hitBuffer);

        for (int i = 0; i < count; i++)
        {
            if(hitBuffer[i].TryGetComponent(out DamagableComponent damagable) 
               && (damagable.Affiliation & searchTargets) != 0)
            {
                Target = damagable;
                break;
            }
        }
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {

        Color handlesColor = UnityEditor.Handles.color;
        CompareFunction handlesZTest = UnityEditor.Handles.zTest;
        
        UnityEditor.Handles.color = new Color(0.6f, 0, 0, 0.1f);
        UnityEditor.Handles.zTest = CompareFunction.LessEqual;
        
        //Draw view cone
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-45, transform.up) * transform.forward, viewCone, viewDist);

        UnityEditor.Handles.color = new Color(1f, 0f, 0.09f, 0.22f);

        //Draw touch radius
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, touchRadius);
        
        UnityEditor.Handles.color = handlesColor;
        UnityEditor.Handles.zTest = handlesZTest;
        


        Color gizmosColor = Gizmos.color;
        Gizmos.color = new Color(0.6f, 0, 0, 0.1f);
        
        if(Target)
            Gizmos.DrawWireSphere(Target.transform.position, 2);

        Gizmos.color = gizmosColor;
    }
    
#endif
}
