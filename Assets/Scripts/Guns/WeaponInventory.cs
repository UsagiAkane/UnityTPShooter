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
    
    public void Pickup(DroppedWeapon dropped)
    {
        if (dropped == null) return;

        if (_currentGun != null)
            DropInternal();

        _currentConfig = dropped.GetConfig;
        int startAmmo = dropped.GetCurrentAmmo;

        SpawnGun(_currentConfig, startAmmo);

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
        dropped.Initialize(_currentConfig, _currentGun.CurrentAmmo);

        gunRB.linearVelocity = GetComponent<Rigidbody>().linearVelocity; //TODO PERFOMANCE ISSUES? Better to do var? And we dont have Rigidbody on WeaponInventory. This left after PlayerWeaponController slice
        gunRB.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        gunRB.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);

        Destroy(gunToDrop.gameObject);

        _currentGun = null;
        _currentConfig = null;
    }
    
    private void SpawnGun(GunConfig config, int ammo)
    {
        Gun gun = Instantiate(config.equipedPF, weaponHolder)
            .GetComponent<Gun>();

        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;

        gun.SetOwner(GetComponentInParent<IDamageInstigator>());
        gun.Initialize(config, ammo);

        _currentGun = gun;

        OnGunEquipped?.Invoke(gun);
    }
}