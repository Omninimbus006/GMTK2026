using System;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public Weapon Weapon { get; private set; }
    
    private UpgradesManager upgradesManager;
    private StatusManager statusManager;
    
    public event Action OnPrimaryFire;
    public event Action OnSecondaryFire;
    public event Action OnReload;
    
    private void Start()
    {
        upgradesManager = GetComponentInParent<UpgradesManager>();
        statusManager = GetComponentInParent<StatusManager>();
    }

    public bool RegisterWeapon(Weapon weapon)
    {
        if (Weapon != null)
        {
            
            return false;
        }

        Weapon = weapon;
        OnPrimaryFire += weapon.OnPrimaryFire;
        OnSecondaryFire += weapon.OnSecondaryFire;
        OnReload += weapon.OnReload;
        return true;
    }

    public bool DeregisterWeapon()
    {
        if (Weapon == null)
        {
            return false;
        }

        OnPrimaryFire -= Weapon.OnPrimaryFire;
        OnSecondaryFire -= Weapon.OnSecondaryFire;
        OnReload -= Weapon.OnReload;
        Weapon = null;
        return true;
    }

    public float ApplyWeaponModifier(float input)
    {
        return input;
    }

    public float GetWeaponModifier()
    {
        return 0;
    }
}
