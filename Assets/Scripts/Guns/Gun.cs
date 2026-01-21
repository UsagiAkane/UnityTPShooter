using System;
using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;
    public event Action<bool> OnReloadStateChanged;

    [SerializeField] protected Transform firePoint;

    protected WeaponRuntimeData runtime;
    protected int maxAmmo;
    //тут чи нижче?
    public int AmmoCurrent => runtime.currentAmmo;
    public int AmmoMax => maxAmmo;

    protected float cooldown;

    protected ObjectPool projectilePool;
    protected GunConfig config;
    protected IDamageInstigator Owner { get; private set; }

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }
    
    public virtual void Initialize(GunConfig cfg, WeaponRuntimeData runtimeData)
    {
        config = cfg;
        maxAmmo = cfg.clipSize;
        runtime = runtimeData ?? new WeaponRuntimeData(maxAmmo);

        OnAmmoAmountChanged?.Invoke(runtime.currentAmmo, maxAmmo);
    }
    
    private void Update()
    {
        //Debug.DrawRay(firePoint.position, firePoint.forward, Color.red);
        TickCooldown(Time.deltaTime);
    }
    
    public virtual void SetOwner(IDamageInstigator owner)
    {
        Owner = owner;
    }

    public bool CanShoot()
    {
        return runtime.currentAmmo > 0 && cooldown <= 0f; // && !runtime.isReloading;
    }
    
    public virtual void Shoot()
    {
        if (!CanShoot()) return;

        GameObject bullet = projectilePool.GetBulletProjectile(
            firePoint.position,
            firePoint.rotation
        );

        bullet.GetComponent<BulletProjectile>().Init(
            firePoint.forward,
            config.projectileSpeed,
            config.damage,
            config.projectileLifeTimeSeconds,
            projectilePool,
            Owner
        );

        ConsumeAmmo();
        cooldown = 60f / config.fireRate;

        SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }
    
    protected virtual void ConsumeAmmo(int amount = 1)
    {
        runtime.currentAmmo -= amount;
        OnAmmoAmountChanged?.Invoke(runtime.currentAmmo, maxAmmo);
    }

    public virtual void Reload()//MonoBehaviour runner)
    {
        // if (runtime.isReloading) return;
        // if (runtime.currentAmmo == maxAmmo) return;
        //
        // runtime.isReloading = true;
        // runtime.reloadVersion++;
        //
        // int myVersion = runtime.reloadVersion;

        OnReloadStateChanged?.Invoke(true);
        StartCoroutine(ReloadRoutine());
        //runner.StartCoroutine(ReloadRoutine(myVersion));
    }

    private IEnumerator ReloadRoutine()
    {
        OnReloadStateChanged?.Invoke(true);
        
        SFXmanager.instance.PlaySFXClip(config.reloadSfx, transform, 1f);

        yield return new WaitForSeconds(config.reloadTime);
        
        runtime.currentAmmo = maxAmmo;

        OnAmmoAmountChanged?.Invoke(runtime.currentAmmo, maxAmmo);
        OnReloadStateChanged?.Invoke(false);
    }
    
    private IEnumerator ReloadRoutine(int version)
    {
        SFXmanager.instance.PlaySFXClip(config.reloadSfx, transform, 1f);

        yield return new WaitForSeconds(config.reloadTime);

        //Якщо був дропнутий
        if (runtime.reloadVersion != version)
            yield break;

        runtime.currentAmmo = maxAmmo;
        runtime.isReloading = false;

        OnAmmoAmountChanged?.Invoke(runtime.currentAmmo, maxAmmo);
        OnReloadStateChanged?.Invoke(false);
    }


    protected void TickCooldown(float dt)
    {
        if (cooldown >= 0f) cooldown -= dt;
    }
}