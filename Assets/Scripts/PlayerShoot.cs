using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootAudioClip;
    
    List<WeaponComponent> weapons;
    int weaponIndex;

    public WeaponComponent Weapon => weapons[weaponIndex];
    public IReadOnlyCollection<WeaponComponent> Weapons => weapons;

    void Awake()
    {
        weapons = new List<WeaponComponent>();
        
        foreach (WeaponComponent weapon in GetComponents<WeaponComponent>())
            weapons.Add(weapon);

        AddWeapon(WeaponType.Melee);
    }

    void Update()
    {
        ProcessChangeWeapon();
        ProcessCanShoot();
        ProcessShootInput();
    }

    public bool DoesContainsWeapon(WeaponType type)
    {
        return weapons.Any(weapon => weapon.Type == type);
    }
    
    public bool AddWeapon(WeaponType type)
    {
        if (type == WeaponType.None)
            return false;

        if (DoesContainsWeapon(type))
            return false;

        WeaponComponent weapon = WeaponHelper.AddWeapon(gameObject, type);
        weapons.Add(weapon);

        return true;
    }

    void ProcessChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && TrySelectWeapon(0))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha2) && TrySelectWeapon(1))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha3) && TrySelectWeapon(2))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha4) && TrySelectWeapon(3))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha5) && TrySelectWeapon(4))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha6) && TrySelectWeapon(5))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha7) && TrySelectWeapon(6))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha8) && TrySelectWeapon(7))
            return;
        if (Input.GetKeyDown(KeyCode.Alpha9) && TrySelectWeapon(8))
            return;
    }

    bool TrySelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count)
            return false;

        weaponIndex = index;
        return true;
    }
    
    void ProcessCanShoot()
    {
        Transform cameraTransform = GameScene.Player.Camera.transform;

        GameScene.Aim.CanShoot = false;
        
        if (!PhysicsUtility.RaycastIgnoreSelf(cameraTransform.position, cameraTransform.forward, transform, out RaycastHit hit)) 
            return;
        
        if(!Weapon.CanAttack())
            return;
        
        //probably check in children 
        if (hit.collider.TryGetComponent(out DamagableComponent damagable))
            GameScene.Aim.CanShoot = !damagable.IsDead && ((damagable.Affiliation & Affiliation.Demons) != 0 || (damagable.Affiliation & Affiliation.Neutral) != 0);
        else
            GameScene.Aim.CanShoot = false;
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
    
        Weapon.Attack(cameraTransform.position, cameraTransform.forward);
    }
    
}
