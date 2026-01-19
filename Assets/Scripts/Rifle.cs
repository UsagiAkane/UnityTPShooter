using System;
using UnityEngine;

public class Rifle : Gun
{
    public override void Initialize(GunConfig cfg)
    {
        base.Initialize(cfg);
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }

    public override void Shoot()
    {
        base.Shoot();
    }
}