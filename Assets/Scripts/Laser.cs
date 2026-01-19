using UnityEngine;

public class Laser : Gun
{
    public override void Initialize(GunConfig cfg)
    {
        base.Initialize(cfg);
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }

    public new void Shoot()
    {
        base.Shoot();
        
        if (Physics.Raycast(firePoint.position, transform.TransformDirection(Vector3.forward),
                out RaycastHit raycastHit, 100f))
        {
            if (raycastHit.collider.TryGetComponent(out BulletTarget bulletTarget)) bulletTarget.TookDamage(50f);
        }
    }
}