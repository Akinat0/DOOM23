using UnityEngine;
using UnityEngine.Rendering;

public abstract class WeaponComponent : MonoBehaviour
{
    [SerializeField] float radius = 10;
    [SerializeField] int ammoCount;

    BaseCharacterController owner;

    public int AmmoCount
    {
        get => ammoCount;
        set => ammoCount = value;
    }
    
    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    public BaseCharacterController Owner => owner;

    void Awake()
    {
        owner = GetComponent<BaseCharacterController>();
    }

    public abstract bool CanShoot();

    public abstract void Shoot(Vector3 origin, Vector3 direction);
    
#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Color handlesColor = UnityEditor.Handles.color;
        CompareFunction handlesZTest = UnityEditor.Handles.zTest;
        
        UnityEditor.Handles.color = new Color(0.0f, 1, 0, 0.1f);
        UnityEditor.Handles.zTest = CompareFunction.LessEqual;

        //Draw touch radius
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, Radius);
        
        UnityEditor.Handles.color = handlesColor;
        UnityEditor.Handles.zTest = handlesZTest;
        
    }
#endif
}