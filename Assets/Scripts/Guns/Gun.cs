using System;
using UnityEngine;

//ган сам вирішує, як реагувати. Якщо це граната, то вона має цілитись по кривій вгору від точки
//1 AimSystem дає дані
//2 Controller каже "стріляй"
//3 Gun стріляє(якщо може)
public abstract class Gun : MonoBehaviour, IAimProvider
{
    public event Action<int, int> OnAmmoAmountChanged;

    // IAimProvider
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform visualRoot;//визначаємо що крутимо
    public Quaternion VisualRotation => visualRoot != null ? visualRoot.rotation : transform.rotation;//публічний аксесор до візуального ротейшна гану. У дропі на його основі спавнимо

    [SerializeField] protected float maxAimDistance = 100f;

    public Transform AimOrigin => firePoint;
    public float MaxAimDistance => maxAimDistance;

    public void OnAimUpdated(AimResult result)
    {
        //Debug.Log($"{name} OnAimUpdated", this);
        RotateTowards(result.AimPoint);
    }
    
    

    // Config / State
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
        TickCooldown(
            Time.deltaTime);//знає про свій cooldown(від firerate), сам його трекає бо він потрібен тільки тут. Не залежить від input бо ми не маємо впливати на максимальний shots per minute
    }

    public void SetOwner(IDamageInstigator instigator)
    {
        owner = instigator;
    }
    
    public void ReloadInstant()//Must be called from state machine after reload finished
    {
        currentAmmo = maxAmmo;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);
    }

    //Shooting API
    public bool CanShoot()//TODO relocate in state machine
    {
        return currentAmmo > 0 && cooldown <= 0f;
    }
    
    public void Shoot(AimResult aim)
    {
        ShootInternal(aim);
    }
    
    private void ShootInternal(AimResult aim)
    {
        if (!CanShoot())
            return;

        //тепер логіка стрільби працює з AimResult
        ShootLogic(aim);//трігер пострілу для логіки проджектайл\хітскан\дробовик
        AfterShoot();
    }

    //тепер вся стрільба залежить від AimResult а не візуального положення гану
    protected abstract void ShootLogic(AimResult aim);//Laser override ShootLogic(), Shotgun override ShootLogic(), Burst rifle override Shoot() або ShootLogic()

    protected virtual void AfterShoot()
    {
        currentAmmo--;
        cooldown = 60f / config.fireRate;
        OnAmmoAmountChanged?.Invoke(currentAmmo, maxAmmo);

        if (config.shotSfx != null)
            SFXmanager.instance.PlaySFXClip(config.shotSfx, transform, 1f);
    }
    
    //Aim visuals
    private void RotateTowards(Vector3 aimPoint)
    {
        Vector3 dir = aimPoint - visualRoot.position;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        visualRoot.rotation = rot;//обирає що крутити (visualRoot/firepoint), може згладжувати, може ігнорувати (наприклад, melee)
    }

    protected void TickCooldown(float dt)
    {
        if (cooldown > 0f)
            cooldown -= dt;
    }
}