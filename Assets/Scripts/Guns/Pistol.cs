using UnityEngine;

public class Pistol : Gun
{
    public override void Initialize(GunConfig cfg, WeaponRuntimeData runtime)
    {
        base.Initialize(cfg, runtime);
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }
}