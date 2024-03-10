using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] UIAim aim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] WeaponComponent weapon;


    void Update()
    {
        ProcessCanShoot();
        ProcessShootInput();
    }


    void ProcessCanShoot()
    {
        Transform cameraTransform = GameScene.Player.Camera.transform;

        aim.CanShoot = false;
        
        if (!PhysicsUtility.RaycastIgnoreSelf(cameraTransform.position, cameraTransform.forward, transform, out RaycastHit hit)) 
            return;
        
        if(!weapon.CanAttack())
            return;
        
        //probably check in children 
        if (hit.collider.TryGetComponent(out DamagableComponent damagable))
            aim.CanShoot = !damagable.IsDead && ((damagable.Affiliation & Affiliation.Demons) != 0 || (damagable.Affiliation & Affiliation.Neutral) != 0);
        else
            aim.CanShoot = false;
    }

    void ProcessShootInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        Transform cameraTransform = GameScene.Player.Camera.transform;

        if (audioSource != null && shootAudioClip != null)
        {
            audioSource.pitch = 1 + Random.Range(-0.05f, 0.05f);
            audioSource.PlayOneShot(shootAudioClip);
        }
    
        weapon.Attack(cameraTransform.position, cameraTransform.forward);
    }
    
    
}
