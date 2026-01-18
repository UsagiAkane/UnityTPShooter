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
        if (!CanShoot) return;
        if (cooldown >= 0f) return;

        GameObject bullet = projectilePool.GetBulletProjectile(firePoint.position, transform.rotation);
        bullet.GetComponent<BulletProjectile>()
            .Init(firePoint.forward, config.projectileSpeed, config.damage, projectilePool);

        CurrentAmmo--;
        cooldown = cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }
}