using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] UIAim aim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] GameObject shootVfxPrefab;
    [SerializeField] int damage;

    void Update()
    {
        ProcessCanShoot();
        ProcessShootInput();
    }


    void ProcessCanShoot()
    {
        Transform cameraTransform = GameScene.Player.Camera.transform;

        aim.CanShoot = false;
        
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit)) 
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

        //we didn't hit anything
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit))
            return;

        if (!hit.collider.TryGetComponent(out DamagableComponent damagable))
        {
            //we've hit wall or something solid
            if (shootVfxPrefab)
                Instantiate(shootVfxPrefab, hit.point, Quaternion.identity);
            return;
        }

        damagable.ApplyDamage(damage);

        
    }
    
    
}
