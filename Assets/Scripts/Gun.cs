using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;

    [SerializeField] protected Transform firePoint;
    protected float cooldown; // time since last shot
    protected ObjectPool projectilePool;
    protected GunConfig config;
    protected bool CanShoot => _currentAmmo > 0 && cooldown <= 0f;

    private int _currentAmmo;

    public int CurrentAmmo
    {
        get => _currentAmmo;
        protected set
        {
            _currentAmmo = value;
            OnAmmoAmountChanged?.Invoke(_currentAmmo, config?.clipSize ?? 0);
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

    public void Shoot()
    {
        if (!CanShoot) return;
        if (cooldown >= 0f) return;

        GameObject bullet = projectilePool.GetBulletProjectile(firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletProjectile>()
            .Init(firePoint.forward, config.projectileSpeed, config.damage, projectilePool);
        
        CurrentAmmo--;
        cooldown = cooldown = 60f / config.fireRate;
        
        //play sfx
        SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }

    public virtual void Reload()
    {
        CurrentAmmo = config?.clipSize ?? 0;
    }

    protected void TickCooldown(float dt)
    {
        if (cooldown >= 0f) cooldown -= dt;
    }
}