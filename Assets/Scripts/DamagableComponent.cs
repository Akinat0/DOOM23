using UnityEngine;

public class DamagableComponent : MonoBehaviour
{
    [SerializeField] Affiliation affiliation = Affiliation.Neutral;
    [SerializeField] int hp = 100;

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

            currentHp = value;

            if(currentHp <= 0)
                Die();
        }
    }

    public void ApplyDamage(int damage)
    {
        Hp -= damage;
    }
    
    void Die()
    {
        Debug.Log($"{gameObject.name} is dead");
        isDead = true;
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
