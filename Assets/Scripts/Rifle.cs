using UnityEngine;

public class Rifle : Gun
{
    public override void Shoot()
    {
        if (!CanShoot) return;
        Debug.Log("\ncurrent ammo = " + CurrentAmmo + "\nconfig name" + config.name);

        projectilePool.GetBulletProjectile(
            transform.position,
            transform.rotation,
            transform.forward,
            config.projectileSpeed);
        CurrentAmmo--;
        //cooldown = 1f / (config.fireRate > 0f ? config.fireRate : 1f);
    }
}