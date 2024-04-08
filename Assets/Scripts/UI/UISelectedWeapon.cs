using TMPro;
using UnityEngine;

public class UISelectedWeapon : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    
    void Update()
    {
        text.text = GameScene.PlayerShoot.Weapon.Type.ToString();
    }
}
