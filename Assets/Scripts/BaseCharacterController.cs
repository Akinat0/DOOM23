using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public abstract class BaseCharacterController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10;
    

    GameObject floor;
    GameObject Floor
    {
        get => floor;
        set
        {
            if (floor != value)
            {
                if (floor != null)
                    floor.SendMessage("OnCharacterExit", this, SendMessageOptions.DontRequireReceiver);
                
                if (value != null)
                    value.SendMessage("OnCharacterEnter", this, SendMessageOptions.DontRequireReceiver);
            }

            floor = value;
        }
    }

    public DamagableComponent Damagable { get; private set; }


    protected virtual void Awake()
    {
        Damagable = GetComponent<DamagableComponent>();
    }

    public abstract float Height { get; }
    public abstract float Radius { get; }

    public float MaxSpeed => maxSpeed;

    public float CurrentSpeed { get; private set; }

    Vector3 prevPos;

    void LateUpdate()
    {
        UpdateCurrentSpeed();
        DetectPlayerCollision();
    }

    void DetectPlayerCollision()
    {
        if (Physics.Linecast(
                transform.position,
                transform.position + Vector3.down * (Height / 2 + 0.1f),
                out RaycastHit hit))
        {
            Floor = hit.collider.gameObject;
            Floor.SendMessage("OnCharacterStay", this, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Floor = null;
        }
    }

    void UpdateCurrentSpeed()
    {
        Vector3 currentPos = transform.position;
        CurrentSpeed = Vector3.Distance(currentPos, prevPos) / Time.deltaTime;
        prevPos = currentPos;
    }
}
