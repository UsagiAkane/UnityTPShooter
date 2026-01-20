using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;

    [SerializeField] protected Transform firePoint;

    protected int currentAmmo;
    protected int maxAmmo;
    //тут чи нижче?
    public int AmmoCurrent => currentAmmo;
    public int AmmoMax => maxAmmo;

    protected float cooldown;

    protected ObjectPool projectilePool;
    protected GunConfig config;
    protected GameObject Owner { get; private set; }

    public virtual void SetOwner(GameObject owner)
    {
        Owner = owner;
    }

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red);
        TickCooldown(Time.deltaTime);
    }

    public virtual void Initialize(GunConfig cfg)
    {
        config = cfg;
        currentAmmo = cfg.clipSize;
        maxAmmo = cfg.clipSize;
        cooldown = 0f;
    }

    public bool CanShoot()
    {
        return (currentAmmo > 0 && cooldown <= 0f);
    }

    public virtual void Shoot()
    {
        if (!CanShoot()) return;

        GameObject bullet = projectilePool.GetBulletProjectile(firePoint.position, firePoint.rotation);

        bullet.GetComponent<BulletProjectile>().Init(
            firePoint.forward,
            config.projectileSpeed,
            config.damage,
            config.projectileLifeTimeSeconds,
            projectilePool,
            Owner);

        ConsumeAmmo(); //OLD: currentAmmo--; with property set override to invoke
        cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }

    protected virtual void ConsumeAmmo(int amount = 1)
    {
        currentAmmo -= amount;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);
    }

    public virtual void Reload()
    {
        currentAmmo = config?.clipSize ?? 0;
    }

    protected void TickCooldown(float dt)
    {
        if (cooldown >= 0f) cooldown -= dt;
    }
}