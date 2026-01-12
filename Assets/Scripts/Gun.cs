using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    protected int currentAmmo;
    protected float cooldown;// time since last shot
    protected ObjectPool projectilePool;
    protected GunConfig config;

    public virtual void Initialize(GunConfig cfg, ObjectPool pool)
    {
        config = cfg;
        projectilePool = pool;
        currentAmmo = cfg.clipSize;
        cooldown = 0f;
    }

    public abstract void Shoot();

    public virtual void Reload()
    {
        currentAmmo = config?.clipSize ?? 0;
    }

    public bool CanShoot => currentAmmo > 0 && cooldown <= 0f;

    protected void TickCooldown(float dt)
    {
        if (cooldown > 0f) cooldown -= dt;
    }

    protected BulletProjectile SpawnBullet(Vector3 position, Quaternion rotation, Vector3 dir, float speed)
    {
        //if (projectilePool == null) return null; ???????????????????????????????????????
        return projectilePool.GetBulletProjectile(position, rotation, dir, speed);
    }
}