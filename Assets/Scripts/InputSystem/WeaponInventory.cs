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
        //Debug.Log($"Picked up: {dropped.GetConfig().weaponName}");
        if (dropped == null) return;

        Equip(dropped.GetConfig());
        Destroy(dropped.gameObject);
    }

    public void Equip(GunConfig config)
    {
        if (_currentGun != null)
            DropInternal();

        
        
        Gun gun = Instantiate(config.equipedPF, weaponHolder)
            .GetComponent<Gun>();

        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;

        gun.Initialize(config);
        gun.SetOwner(GetComponentInParent<IDamageInstigator>());

        _currentGun = gun;
        _currentConfig = config;

        OnGunEquipped?.Invoke(gun);
    }

    public void Drop()
    {
        if (_currentGun == null) return;
        DropInternal();
    }

    private void DropInternal(float dropImpulse = 2f, float torqueRange = 1f)
    {
        Gun gunToDrop = _currentGun;
        GunConfig configToDrop = _currentConfig;

        OnGunUnequipped?.Invoke(gunToDrop);

        _currentGun = null;
        _currentConfig = null;

        Vector3 spawnPos =
            transform.position +
            transform.forward * 0.5f +
            transform.right * 0.5f;

        Rigidbody rb = Instantiate(
            configToDrop.droppepPF,
            spawnPos,
            gunToDrop.transform.rotation
        ).GetComponent<Rigidbody>();

        rb.linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        rb.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        rb.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);

        Destroy(gunToDrop.gameObject);
    }
}
