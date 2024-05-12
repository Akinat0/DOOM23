using UnityEngine;

public class GameScene : MonoBehaviour
{
    static GameScene instance;

    [SerializeField] PlayerController player;
    [SerializeField] PuffFactory puffFactory;
    [SerializeField] PuffFactory bfgPuffFactory;
    [SerializeField] PrefabFactory bfgProjectileFactory;
    [SerializeField] UIVignette vignette;
    [SerializeField] UIAim aim;

    PlayerShoot playerShoot;
    public static PlayerController Player => instance != null ? instance.player : null;
    public static PlayerShoot PlayerShoot => instance != null ? instance.playerShoot : null;
    public static PuffFactory PuffFactory => instance != null ? instance.puffFactory : null;
    public static PuffFactory BFGPuffFactory => instance != null ? instance.bfgPuffFactory : null;
    public static PrefabFactory BFGProjectileFactory => instance != null ? instance.bfgProjectileFactory : null;
    public static UIVignette Vignette => instance != null ? instance.vignette : null;
    public static UIAim Aim => instance != null ? instance.aim : null;

    void Awake()
    {
        instance = this;
        playerShoot = player.GetComponent<PlayerShoot>();
        
    }

    void OnDestroy()
    {
        instance = null;
    }
}
