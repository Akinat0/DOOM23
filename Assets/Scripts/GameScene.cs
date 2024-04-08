using UnityEngine;

public class GameScene : MonoBehaviour
{
    static GameScene instance;

    [SerializeField] PlayerController player;
    [SerializeField] PuffFactory puffFactory;
    [SerializeField] UIVignette vignette;

    PlayerShoot playerShoot;
    public static PlayerController Player => instance != null ? instance.player : null;
    public static PlayerShoot PlayerShoot => instance != null ? instance.playerShoot : null;
    public static PuffFactory PuffFactory => instance != null ? instance.puffFactory : null;
    public static UIVignette Vignette => instance != null ? instance.vignette : null; 

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
