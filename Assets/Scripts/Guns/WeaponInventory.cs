using System;
using UnityEngine;

public class WeaponInventory : MonoBehaviour, IWeaponInventory
{
    public Gun CurrentGun => _currentGun;
    
    public event Action<Gun> OnGunEquipped;
    public event Action<Gun> OnGunUnequipped;

    [SerializeField] private Transform weaponHolder;

    private Gun _currentGun;
    private GunConfig _currentConfig;
    private WeaponRuntimeData _currentRuntime;
    
    public void Pickup(DroppedWeapon dropped)
    {
        if (dropped == null) return;

        if (_currentGun != null)
            DropInternal();

        _currentConfig = dropped.GetConfig();
        _currentRuntime = dropped.GetRuntime();

        if (_currentRuntime == null)
            _currentRuntime = new WeaponRuntimeData(_currentConfig.clipSize);

        SpawnGun(_currentConfig, _currentRuntime);

        Destroy(dropped.gameObject);
    }

    public void Drop()
    {
        if (_currentGun == null) return;
        DropInternal();
    }

    private void DropInternal(float dropImpulse = 2f, float torqueRange = 1f)//TODO torqueRange
    {
        Gun gunToDrop = _currentGun;
        
        //відмінити reload
        _currentRuntime.reloadVersion++;
        _currentRuntime.isReloading = false; //TODO REMOVE THIS

        OnGunUnequipped?.Invoke(gunToDrop);

        Vector3 spawnPos =
            transform.position +
            transform.forward * 0.5f +
            transform.right * 0.5f;

        Rigidbody gunRB = Instantiate(
            _currentConfig.droppepPF,
            spawnPos,
            gunToDrop.transform.rotation
        ).GetComponent<Rigidbody>();

        DroppedWeapon dropped = gunRB.GetComponent<DroppedWeapon>();
        dropped.Initialize(_currentConfig, _currentRuntime);

        gunRB.linearVelocity = GetComponent<Rigidbody>().linearVelocity; //TODO PERFOMANCE ISSUES? Better to do var? And we dont have Rigidbody on WeaponInventory. This left after PlayerWeaponController slice
        gunRB.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        gunRB.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);

        Destroy(gunToDrop.gameObject);

        _currentGun = null;
        _currentConfig = null;
        _currentRuntime = null;
    }
    
    private void SpawnGun(GunConfig config, WeaponRuntimeData runtime)//TODO Override with new ammo system
    {
        Gun gun = Instantiate(config.equipedPF, weaponHolder)
            .GetComponent<Gun>();

        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;

        gun.SetOwner(GetComponentInParent<IDamageInstigator>());
        gun.Initialize(config, runtime);

        _currentGun = gun;

        OnGunEquipped?.Invoke(gun);
    }
}