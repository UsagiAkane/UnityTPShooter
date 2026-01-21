using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;

    [SerializeField] protected Transform firePoint;

    protected GunConfig config;
    protected IDamageInstigator owner;

    protected int currentAmmo;
    protected int maxAmmo;

    protected float cooldown;

    protected ObjectPool projectilePool;

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    protected virtual void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    public virtual void Initialize(GunConfig cfg, int startAmmo)
    {
        config = cfg;
        maxAmmo = cfg.clipSize;
        currentAmmo = Mathf.Clamp(startAmmo, 0, maxAmmo);

        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);
    }
    
    private void Update()
    {
        TickCooldown(Time.deltaTime); //знає про свій cooldown(від firerate), сам його трекає бо він потрібен тільки тут. Не залежить від input бо ми не маємо впливати на максимальний shots per minute
    }

    public void SetOwner(IDamageInstigator instigator)
    {
        owner = instigator;
    }
    

    public bool CanShoot()//TODO relocate in state machine
    {
        return currentAmmo > 0 && cooldown <= 0f;
    }

    public virtual void Shoot()
    {
        if (!CanShoot())
            return;

        ShootLogic();//трігер пострілу для логіки проджектайл\хітскан\дробовик
        AfterShoot();
    }
    protected virtual void ShootLogic()//Laser override ShootLogic(), Shotgun override ShootLogic(), Burst rifle override Shoot() або ShootLogic()
    {
        SpawnProjectile();
    }
    protected virtual void AfterShoot()
    {
        currentAmmo--;
        cooldown = 60f / config.fireRate;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);

        if (config.shotSfx != null)
            SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }

    public void ReloadInstant()//Must be called from state machine after reload finished
    {
        currentAmmo = maxAmmo;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);
    }
    
    protected void SpawnProjectile()
    {
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
            owner
        );
    }
    
    protected void TickCooldown(float dt)
    {
        if (cooldown > 0f)
            cooldown -= dt;
    }
    
    // //DONT NEED IT RIGHT NOW
    // protected void SpawnShot()
    // {
    //     if (config.usesProjectile)
    //         SpawnProjectile();
    //     else
    //         DoHitscan();
    // }
}