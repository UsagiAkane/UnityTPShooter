using System;
using UnityEngine;

public interface IWeaponInventory
{
    Gun CurrentGun { get; }

    event Action<Gun> OnGunEquipped;
    event Action<Gun> OnGunUnequipped;

    //void Equip(GunConfig config);
    void Drop();
}

