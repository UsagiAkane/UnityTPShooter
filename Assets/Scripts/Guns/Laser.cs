using UnityEngine;

namespace Guns
{
    public class Laser : HitscanGun
    {
        public override void Initialize(GunConfig cfg, int ammo)
        {
            base.Initialize(cfg, ammo);

            //якщо пул треба буде для ФХ то краще створити окремий
            //rojectilePool.InitializePool(config.bulletPF, config.usesProjectile);
        }

        protected override void OnHit(RaycastHit hit, Vector3 direction)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageInfo damageInfo = new DamageInfo
                {
                    amount = config.damage,
                    source = gameObject,   // gun
                    instigator = owner,    // holder
                    hitPoint = hit.point,
                    hitDirection = direction
                };

                damageable.TakeDamage(damageInfo);
            }

            //TODO impact FX
        }

        protected override void OnMiss(Vector3 direction)
        {
            //TODO FX without hit
        }
    }
}