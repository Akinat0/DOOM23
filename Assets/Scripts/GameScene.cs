using UnityEngine;

public class GameScene : MonoBehaviour
{
    static GameScene instance;

    [SerializeField] PlayerController player;
    [SerializeField] PuffFactory puffFactory;

    public static PlayerController Player => instance != null ? instance.player : null; 
    public static PuffFactory PuffFactory => instance != null ? instance.puffFactory : null; 

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }
}
