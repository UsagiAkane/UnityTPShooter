using System;
using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;
    public event Action<bool> OnReloadStateChanged;

    [SerializeField] protected Transform firePoint;

    protected int currentAmmo;
    protected int maxAmmo;
    protected bool isReloading;
    //тут чи нижче?
    public int AmmoCurrent => currentAmmo;
    public int AmmoMax => maxAmmo;

    protected float cooldown;

    protected ObjectPool projectilePool;
    protected GunConfig config;
    protected IDamageInstigator Owner { get; private set; }

    public virtual void SetOwner(IDamageInstigator owner)
    {
        Owner = owner;
    }

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        //Debug.DrawRay(firePoint.position, firePoint.forward, Color.red);
        TickCooldown(Time.deltaTime);
    }

    public virtual void Initialize(GunConfig cfg)
    {
        config = cfg;
        currentAmmo = cfg.clipSize;
        maxAmmo = cfg.clipSize;
        cooldown = 0f;
        
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);
    }

    public bool CanShoot()
    {
        return currentAmmo > 0 && cooldown <= 0f && !isReloading;
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

    public virtual void Reload(MonoBehaviour runner)
    {
        if (isReloading) return;
        if (currentAmmo == maxAmmo) return;

        runner.StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        SFXmanager.instance.PlaySFXClip(config.reloadSfx, transform, 1f);
            
        isReloading = true;
        OnReloadStateChanged?.Invoke(true);


        yield return new WaitForSeconds(config.reloadTime);

        currentAmmo = maxAmmo;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);

        isReloading = false;
        OnReloadStateChanged?.Invoke(false);
    }

    protected void TickCooldown(float dt)
    {
        if (cooldown >= 0f) cooldown -= dt;
    }
}