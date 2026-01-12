using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        if (!CanShoot) return;
        //Vector3 dir = direction.normalized;
        //Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        //SpawnBullet(origin, rot, dir, config.projectileSpeed);
        //Vector3 aimDir = (_mouseWorldPosition-spawnBulletPosition.position).normalized;
        //Quaternion rot = Quaternion.LookRotation(aimDir, Vector3.up);
        projectilePool.GetBulletProjectile(
            this.transform.forward, 
            this.transform.localRotation, 
            this.transform.forward,
            config.projectileSpeed);
        currentAmmo--;
        cooldown = 1f / (config.fireRate > 0f ? config.fireRate : 1f);
    }
}