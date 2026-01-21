using Player.AimSystem;
using UnityEngine;

namespace Guns
{
    public abstract class ProjectileGun : Gun
    {
        [Header("Projectile params")]
        [SerializeField] protected float projectileSpeed = 20f;
        [SerializeField] protected float projectileLifetime = 5f;

        [SerializeField] protected ObjectPool projectilePool;
        
        //Init pool
        public override void Initialize(GunConfig cfg, int startAmmo)
        {
            base.Initialize(cfg, startAmmo);

            projectilePool.InitializePool(
                cfg.bulletPF,
                cfg.usesProjectile
            );
        }
        
        protected override void ShootLogic(AimResult aim)
        {
            Vector3 dirFromFirePoint = (aim.AimPoint - firePoint.position).normalized;
            SpawnProjectile(dirFromFirePoint);
        }
        
        protected virtual Vector3 GetDirection(AimResult aim)
        {
            return aim.Direction;
        }

        protected virtual void SpawnProjectile(Vector3 direction)
        {
            GameObject projectile = projectilePool.GetBulletProjectile(firePoint.position, Quaternion.LookRotation(direction));

            if (!projectile.TryGetComponent(out BulletProjectile bullet))
            {
                Debug.Log("BulletProjectile missing");
                return;
            }
            
            bullet.Init(
                direction,
                projectileSpeed,
                damage,//runtime snapshot
                projectileLifetime,
                projectilePool,
                instigator
            );
        }
    }
}