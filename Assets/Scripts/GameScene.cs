using System;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    static GameScene instance;

    [SerializeField] PlayerController player;

    public static PlayerController Player => instance != null ? instance.player : null; 

    
    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }
}
