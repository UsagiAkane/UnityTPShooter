using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private AudioClip shotSound;
    
    public override void Shoot()
    {
        if (!CanShoot) return;
        if (cooldown >= 0.01f) return;
        //Debug.Log("\ncurrent ammo = " + CurrentAmmo + "\nconfig name" + config.name);

        projectilePool.GetBulletProjectile(
            firePoint.position,
            transform.rotation,
            transform.forward,
            config.projectileSpeed);
        CurrentAmmo--;

        cooldown = cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(shotSound, transform, 1f);
    }
}