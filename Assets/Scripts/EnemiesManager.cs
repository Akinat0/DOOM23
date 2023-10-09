
using System.Collections.Generic;

public class EnemiesManager
{
    readonly HashSet<Enemy> enemies = new HashSet<Enemy>();

    public IReadOnlyCollection<Enemy> Enemies => enemies;

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    
    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}