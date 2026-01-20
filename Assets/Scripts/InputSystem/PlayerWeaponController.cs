using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerWeaponController : MonoBehaviour
{
    public static event Action<int, int> OnPlayerAmmoChanged;
    public static event Action<Gun> OnGunPickUp;
    public static event Action<Gun> OnGunDropDown;

    [Header("System")] //
    [SerializeField]
    private Transform weaponHolder; // where to attach weapon

    [SerializeField] private LayerMask aimCollisionLayerMask;
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    [Header("Actions")] //
    [SerializeField]
    private InputActionReference shootAction;

    [SerializeField] private InputActionReference pickUpAction;
    [SerializeField] private InputActionReference weaponDropAction;

    private Gun _currentGun;
    private GunConfig _currentGunConfig;
    private Vector3 _mouseWorldPosition = Vector3.zero;

    private void Update()
    {
        if (pickUpAction.action.triggered) TryEquipFromLook();

        if (weaponDropAction.action.triggered) DropCurrentGun();

        _mouseWorldPosition = HandleAimPosition();

        AlignGun();

        if (shootAction.action.IsPressed()) Shoot();
    }

    private void Shoot()
    {
        if (_currentGun == null) return;
        _currentGun.Shoot();
    }

    private Vector3 HandleAimPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimCollisionLayerMask))
            return raycastHit.point;


        return Vector3.zero;
    }

    private void AlignGun()
    {
        if (_currentGun == null) return;
        weaponHolder.LookAt(_mouseWorldPosition);
    }

    private void TryEquipFromLook()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, pickupCollisionLayerMask))
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, 999f, pickupCollisionLayerMask))
                return;

            if (!hit.collider.TryGetComponent<DroppedWeapon>(out var droppedWeapon))
                return;
            
            if (_currentGun != null)
                DropCurrentGun();

            GunConfig config = droppedWeapon.GetConfig();
            Destroy(droppedWeapon.gameObject);

            Gun gunInstance = Instantiate(config.equipedPF, weaponHolder).GetComponent<Gun>();

            gunInstance.transform.localPosition = Vector3.zero;
            gunInstance.transform.localRotation = Quaternion.identity;
            gunInstance.transform.localScale = Vector3.one;

            gunInstance.Initialize(config);
            gunInstance.SetOwner(gameObject);

            EquipGun(gunInstance, config);
        }
    }
    
    private void EquipGun(Gun newGun, GunConfig config)
    {
        if (_currentGun != null)
            UnequipCurrentGun();

        _currentGun = newGun;
        _currentGunConfig = config;

        _currentGun.OnAmmoAmountChanged += HandleGunAmmoChanged;

        HandleGunAmmoChanged(
            _currentGun.AmmoCurrent,
            _currentGun.AmmoMax
        );

        OnGunPickUp?.Invoke(_currentGun);
    }

    private void DropCurrentGun(float dropImpulse = 2f, float torqueRange = 1f)
    {
        if (_currentGun == null) return;

        Vector3 spawnPos =
            transform.position +
            transform.forward * 0.5f +
            transform.right * 0.5f;

        Quaternion spawnRot = _currentGun.transform.rotation;

        Rigidbody droppedRB = Instantiate(_currentGunConfig.droppepPF, spawnPos, spawnRot).GetComponent<Rigidbody>();

        droppedRB.linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        droppedRB.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        droppedRB.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);

        Vector3 randomTorque = new(
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange * 0.05f, torqueRange * 0.05f)
        );

        droppedRB.AddTorque(randomTorque, ForceMode.Impulse);

        UnequipCurrentGun();
    }
    
    private void UnequipCurrentGun()
    {
        if (_currentGun == null) return;

        _currentGun.OnAmmoAmountChanged -= HandleGunAmmoChanged;

        OnGunDropDown?.Invoke(_currentGun);

        Destroy(_currentGun.gameObject);

        _currentGun = null;
        _currentGunConfig = null;
    }

    private void HandleGunAmmoChanged(int current, int max)
    {
        OnPlayerAmmoChanged?.Invoke(current, max);
    }
    
}