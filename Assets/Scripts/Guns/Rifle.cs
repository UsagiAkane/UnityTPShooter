using System;
using UnityEngine;

public class Rifle : Gun
{
    public override void Initialize(GunConfig cfg, int ammo)
    {
        base.Initialize(cfg, ammo);
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }
}