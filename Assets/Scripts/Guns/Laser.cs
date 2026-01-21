using UnityEngine;

public class Laser : Gun
{
    [SerializeField] private float maxRange = 100f;

    public override void Initialize(GunConfig cfg, int ammo)
    {
        base.Initialize(cfg, ammo);

        // Laser не projectile, але пул для FX
        projectilePool.InitializePool(config.bulletPF, config.usesProjectile);
    }

    protected override void ShootLogic()
    {
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
                    instigator = owner, //gun_holder_root
                    hitPoint = hit.point,
                    hitDirection = direction
                };

                damageable.TakeDamage(damageInfo);
            }

            //TODO FX
        }
    }
}