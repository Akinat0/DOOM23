using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] ShooterController shooter;
    
    readonly EnemiesManager enemiesManager = new EnemiesManager();
    
    void Awake()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            enemy.EnemiesManager = enemiesManager;

        shooter.EnemiesManager = enemiesManager;
    }
}
