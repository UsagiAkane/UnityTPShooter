using System;
using UnityEngine;

//TODO Useless interface? delete it because it duplicates code without purpouse
public interface IWeaponInventory
{
    Gun CurrentGun { get; }

    event Action<Gun> OnGunEquipped;
    event Action<Gun> OnGunUnequipped;

    //void Equip(GunConfig config);
    void Drop();
}

