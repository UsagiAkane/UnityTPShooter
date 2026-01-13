using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        if (!CanShoot) return;
        Debug.Log("\ncurrent ammo = "+currentAmmo +"\nconfig name" + config.name);
        
        projectilePool.GetBulletProjectile(
            transform.position, 
            transform.rotation, 
            transform.forward,
            config.projectileSpeed);
        currentAmmo--;
        //cooldown = 1f / (config.fireRate > 0f ? config.fireRate : 1f);
    }
}