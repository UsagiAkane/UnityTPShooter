using System;
using UnityEngine;

public interface IWeaponInputSource
{
    event Action Shoot;
    event Action Reload;
    event Action Pickup;
    event Action Drop;
    event Action Swap;
}