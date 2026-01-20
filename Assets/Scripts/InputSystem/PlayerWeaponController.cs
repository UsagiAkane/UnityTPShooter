using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerWeaponController : MonoBehaviour
{
    public static event Action<int, int> OnPlayerAmmoChanged;
    public static event Action<Gun> OnGunPickUp;
    public static event Action<Gun> OnGunDropDown;

    [Header("System")]
    [SerializeField]private Transform weaponHolder; //where to attach weapon
    [SerializeField] private LayerMask aimCollisionLayerMask;
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    [Header("Actions")]
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private InputActionReference pickUpAction;
    [SerializeField] private InputActionReference weaponDropAction;

    private Gun _currentGun;
    private GunConfig _currentGunConfig;
    private Vector3 _mouseWorldPosition = Vector3.zero;

    private void Update()
    {
        if (pickUpAction.action.triggered)
            TryPickupFromLook();

        if (weaponDropAction.action.triggered)
            TryDropInput();
        
        if (reloadAction.action.triggered)
            TryReload();

        UpdateAim();
        AlignGun();

        if (shootAction.action.IsPressed())
            Shoot();
    }

    private void TryPickupFromLook()
    {
        Vector2 center = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(center);

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, pickupCollisionLayerMask))
            return;

        if (!hit.collider.TryGetComponent<DroppedWeapon>(out var dropped))
            return;

        //replace
        if (_currentGun != null)
            DropGunWorld();

        GunConfig config = dropped.GetConfig();
        Destroy(dropped.gameObject); //world droppedGun destroy

        Gun gunInstance = Instantiate(config.equipedPF, weaponHolder)
            .GetComponent<Gun>();

        gunInstance.transform.localPosition = Vector3.zero;
        gunInstance.transform.localRotation = Quaternion.identity;
        gunInstance.transform.localScale = Vector3.one;

        gunInstance.Initialize(config);
        gunInstance.SetOwner(transform.root.GetComponent<IDamageInstigator>());

        EquipGunState(gunInstance, config);
    }

    private void TryDropInput()
    {
        if (_currentGun == null) return;
        DropGunWorld();
    }

    private void EquipGunState(Gun gun, GunConfig config)
    {
        _currentGun = gun;
        _currentGunConfig = config;

        _currentGun.OnAmmoAmountChanged += HandleGunAmmoChanged;

        HandleGunAmmoChanged(
            _currentGun.AmmoCurrent,
            _currentGun.AmmoMax
        );

        OnGunPickUp?.Invoke(_currentGun);
    }

    private Gun UnequipGunState()
    {
        if (_currentGun == null) return null;

        Gun removedGun = _currentGun;

        _currentGun.OnAmmoAmountChanged -= HandleGunAmmoChanged;
        OnGunDropDown?.Invoke(_currentGun);

        _currentGun = null;
        _currentGunConfig = null;

        return removedGun;
    }

    private void DropGunWorld(float dropImpulse = 2f, float torqueRange = 1f)
    {
        if (_currentGun == null) return;

        //зберігаємо дані ДО unequip
        Gun gunToDrop = _currentGun;
        GunConfig configToDrop = _currentGunConfig;

        //teardown state
        UnequipGunState();

        Vector3 spawnPos =
            transform.position +
            transform.forward * 0.5f +
            transform.right * 0.5f;

        Quaternion spawnRot = gunToDrop.transform.rotation;

        Rigidbody rb = Instantiate(
            configToDrop.droppepPF,
            spawnPos,
            spawnRot
        ).GetComponent<Rigidbody>();

        rb.linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        rb.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        rb.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);

        Vector3 torque = new(
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange * 0.05f, torqueRange * 0.05f)
        );

        rb.AddTorque(torque, ForceMode.Impulse);

        DestroyGunInstance(gunToDrop);
    }

    private void DestroyGunInstance(Gun gun)
    {
        if (gun == null) return;
        Destroy(gun.gameObject);
    }

    private void UpdateAim()
    {
        Vector2 center = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(center);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimCollisionLayerMask))
            _mouseWorldPosition = hit.point;
    }

    private void AlignGun()
    {
        if (_currentGun == null) return;
        weaponHolder.LookAt(_mouseWorldPosition);
    }


    private void Shoot()
    {
        if (_currentGun == null) return;
        _currentGun.Shoot();
    }

    private void HandleGunAmmoChanged(int current, int max)
    {
        OnPlayerAmmoChanged?.Invoke(current, max);
    }
    
    private void TryReload()
    {
        if (_currentGun == null) return;
        _currentGun.Reload(this);
    }
}