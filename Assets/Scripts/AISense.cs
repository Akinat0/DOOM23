
using System;
using UnityEngine;

public class AISense : MonoBehaviour
{
    [SerializeField] float viewCone = 60;
    [SerializeField] float viewDist = 10;
    [SerializeField] Affiliation searchTargets = Affiliation.All;

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
    
    void Update()
    {
        Target = DamagableManager.GetFirstVisibleTarget(searchTargets, transform, viewCone, viewDist);
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {

        Color handlesColor = UnityEditor.Handles.color;
        
        UnityEditor.Handles.color = new Color(0.6f, 0, 0, 0.1f);
        
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-45, transform.up) * transform.forward, viewCone, viewDist);

        UnityEditor.Handles.color = handlesColor;

        if(Target)
            Gizmos.DrawWireSphere(Target.transform.position, 2);
    }
    
#endif
}
