using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event Action<int, int> OnAmmoAmountChanged;

    [SerializeField] protected Transform firePoint;
    protected float cooldown; // time since last shot
    protected ObjectPool projectilePool;
    protected GunConfig config;

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
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red);
        TickCooldown(Time.deltaTime);
    }

    public virtual void Initialize(GunConfig cfg)
    {
        config = cfg;
        CurrentAmmo = cfg.clipSize;
        cooldown = 0f;
    }

    public bool CanShoot()
    {
        return (_currentAmmo > 0 && cooldown <= 0f);
    }

    public virtual void Shoot()
    {
        if (!CanShoot()) return;

        GameObject bullet = projectilePool.GetBulletProjectile(firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletProjectile>()
            .Init(firePoint.forward, config.projectileSpeed, config.damage,config.projectileLifeTimeSeconds, projectilePool);

        CurrentAmmo--;
        cooldown = 60f / config.fireRate;

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