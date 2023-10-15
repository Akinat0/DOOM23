using UnityEngine;
using UnityEngine.UI;

public class UIAim : MonoBehaviour
{
    [SerializeField] Image aimImage;

    public bool CanShoot { get; set; }

    private void Update()
    {
        aimImage.color = CanShoot ? Color.red : Color.white;
    }
}
