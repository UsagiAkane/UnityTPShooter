using System;
using UnityEngine;

[Serializable]
public class WeaponRuntimeData
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