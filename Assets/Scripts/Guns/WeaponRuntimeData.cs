using System;
using UnityEngine;

[Serializable]
public class WeaponRuntimeData //DELETE, replace with new ammo system
{
    public int currentAmmo;
    public bool isReloading;

    public int reloadVersion;

    public WeaponRuntimeData(int maxAmmo)
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        reloadVersion = 0;
    }
}

public class WeaponRuntimeData : MonoBehaviour
{
    [SerializeField] private int currentAmmo;

    public int CurrentAmmo
    {
        get => currentAmmo;
        private set => currentAmmo = value;
    }

    public void SetAmmo(int ammo)
    {
        CurrentAmmo = ammo;
    }
}