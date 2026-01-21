using UnityEngine;

public class Laser : Gun
{
    [SerializeField] private float maxRange = 100f;

    public override void Initialize(GunConfig cfg, WeaponRuntimeData runtime)//TODO delete WeaponRuntimeData. Override with new ammo system
    {
        base.Initialize(cfg, runtime);

        // Laser не projectile, але пул для FX
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;

        base.Shoot();//ammo, cooldown, events

        Vector3 origin = firePoint.position;
        Vector3 direction = firePoint.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxRange))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageInfo damageInfo = new DamageInfo
                {
                    amount = config.damage,
                    source = gameObject, //gun
                    instigator = Owner, //gun_holder_root
                    hitPoint = hit.point,
                    hitDirection = direction
                };

                damageable.TakeDamage(damageInfo);
            }

            //TODO FX
        }
    }
}