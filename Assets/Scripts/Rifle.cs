using System;
using UnityEngine;

public class Rifle : Gun
{
    public override void Shoot()
    {
        if (!CanShoot) return;
        if (cooldown >= 0.01f) return;

        Debug.Log("\ncurrent ammo = " + CurrentAmmo + "\nconfig name" + config.name);

        projectilePool.GetBulletProjectile(
            transform.position,
            transform.rotation,
            transform.forward,
            config.projectileSpeed);
        CurrentAmmo--;
        
        cooldown = cooldown = 60f / config.fireRate;
    }


}