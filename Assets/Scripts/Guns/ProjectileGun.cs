using Player.AimSystem;
using UnityEngine;

namespace Guns
{
    public abstract class ProjectileGun : Gun
    {
        [Header("Projectile params")]
        [SerializeField] protected float projectileSpeed = 20f;
        [SerializeField] protected float projectileLifetime = 5f;

        protected override void ShootLogic(AimResult aim)
        {
            Vector3 direction = GetDirection(aim);

            SpawnProjectile(direction);
        }
        
        protected virtual Vector3 GetDirection(AimResult aim)
        {
            return aim.Direction;
        }

        protected virtual void SpawnProjectile(Vector3 direction)
        {
            if (projectilePool == null)
            {
                Debug.Log($"{name} projectilePool null");
                return;
            }
            
            GameObject projectile = projectilePool.GetBulletProjectile(
                firePoint.position,
                Quaternion.LookRotation(direction)
            );

            projectile.GetComponent<BulletProjectile>().Init(
                direction,
                projectileSpeed,
                config.damage,
                projectileLifetime,
                projectilePool,
                owner
            );
        }
    }
}