using System;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public event Action OnDropRequested;
    public Gun CurrentGun => _currentGun;
    
    public event Action<Gun> OnGunEquipped;
    public event Action<Gun> OnGunUnequipped;

    [SerializeField] private Transform weaponHolder;

    private Gun _currentGun;
    private GunConfig _currentConfig;
    
    private void RequestDrop()
    {
        OnDropRequested?.Invoke();
    }
    
    public void Pickup(DroppedWeapon dropped)
    {
        if (dropped == null) return;

        if (_currentGun != null)
            RequestDrop(); //без фізики, хто дропає? PlayerWeaponController

        _currentConfig = dropped.GetConfig;
        int startAmmo = dropped.GetCurrentAmmo;

        SpawnGun(_currentConfig, startAmmo);

        Destroy(dropped.gameObject);
    }

    public void Drop(Vector3 startingVelocity)
    {
        if (_currentGun == null) return;
        DropInternal(startingVelocity);
    }

    private void DropInternal(Vector3 startingVelocity, float dropImpulse = 2f)
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
            gunToDrop.VisualRotation
        ).GetComponent<Rigidbody>();

        DroppedWeapon dropped = gunRB.GetComponent<DroppedWeapon>();
        dropped.Initialize(_currentConfig, _currentGun.CurrentAmmo);

        //inventory не шукає Rigidbody — воно просто використовує дані
        gunRB.linearVelocity = startingVelocity;//Хто тепер передає velocity? PlayerWeaponController
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