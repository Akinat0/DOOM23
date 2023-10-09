using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public class Enemy : MonoBehaviour
{
    public EnemiesManager EnemiesManager { get; set; }

    void OnEnable()
    {
        EnemiesManager.RegisterEnemy(this);
    }
    
    void OnDisable()
    {
        EnemiesManager.UnregisterEnemy(this);
    }
    
    
}
