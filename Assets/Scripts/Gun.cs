using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;

    private int currentAmmo;
    protected float cooldown; // time since last shot
    protected ObjectPool projectilePool;
    protected GunConfig config;
    protected bool CanShoot => currentAmmo > 0 && cooldown <= 0f;

    public int CurrentAmmo
    {
        get => currentAmmo;
        protected set
        {
            currentAmmo = value;
            OnAmmoAmountChanged?.Invoke(currentAmmo, config?.clipSize ?? 0);
        }
    }

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        TickCooldown(Time.deltaTime);
    }

    public virtual void Initialize(GunConfig cfg)
    {
        config = cfg;
        CurrentAmmo = cfg.clipSize;
        cooldown = 0f;
    }

    public abstract void Shoot();

    public virtual void Reload()
    {
        CurrentAmmo = config?.clipSize ?? 0;
    }

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