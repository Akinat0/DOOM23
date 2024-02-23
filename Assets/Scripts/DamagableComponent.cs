using System;
using UnityEngine;

public class DamagableComponent : MonoBehaviour
{
    [SerializeField] Affiliation affiliation = Affiliation.Neutral;
    [SerializeField] int hp = 100;

    public event Action<int, BaseCharacterController> HpChangedFromCharacter;  
    public event Action<int> HpChanged;  
    public event Action Killed;  

    int currentHp;
    bool isDead;

    public Affiliation Affiliation
    {
        get => affiliation;
        set => affiliation = value;
    }

    private void Start()
    {
        currentHp = hp;
    }

    public bool IsDead => isDead;

    public int Hp
    {
        get => currentHp;
        set
        {
            if (isDead)
                return;

            currentHp = Mathf.Max(value, 0);
            HpChanged?.Invoke(currentHp);

            if(currentHp <= 0)
                Die();
        }
    }

    public void ApplyDamage(int damage, BaseCharacterController source = null)
    {
        if (IsDead)
        {
            Debug.LogWarning("Dead damagable can't apply damage", gameObject);
            return;
        }
        
        if(source != null)
            HpChangedFromCharacter?.Invoke(damage, source);
        
        Hp -= damage;
    }
    
    void Die()
    {
        Debug.Log($"{gameObject.name} is dead");
        isDead = true;
        Killed?.Invoke();
    }

    private void OnEnable()
    {
        DamagableManager.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        DamagableManager.UnregisterEnemy(this);
    }
}
