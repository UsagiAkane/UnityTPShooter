using UnityEngine;

public class Laser : Gun
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

        if (Physics.Raycast(firePoint.position, transform.TransformDirection(Vector3.forward),
                out RaycastHit raycastHit, 100f))
        {
            Debug.DrawRay(firePoint.position, raycastHit.point, Color.red);
            if (raycastHit.collider.TryGetComponent(out BulletTarget bulletTarget))
            {
                //Debug.Log("Target hit");
                bulletTarget.TookDamage(50f);
            }
            else
            {
                Debug.Log("groud hit");
            }
        }

        CurrentAmmo--;
        cooldown = cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }
}