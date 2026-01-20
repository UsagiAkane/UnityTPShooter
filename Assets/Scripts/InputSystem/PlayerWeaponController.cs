using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    private Gun _currentGun;

    private PlayerWeaponInput _input;
    private WeaponInventory _inventory;
    private AimSystem _aimSystem;

    private void Awake()
    {
        _inventory = GetComponent<WeaponInventory>();
        _aimSystem = GetComponent<AimSystem>();
        _input = GetComponent<PlayerWeaponInput>();

        // Inventory to Aim
        _inventory.OnGunEquipped += HandleGunEquipped;
        _inventory.OnGunUnequipped += HandleGunUnequipped;

        // Input
        _input.PickupPressed += TryPickupFromLook;
        _input.ShootPressed += Shoot;
        _input.ReloadPressed += Reload;
        _input.DropPressed += Drop;
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnGunEquipped -= HandleGunEquipped;
            _inventory.OnGunUnequipped -= HandleGunUnequipped;
        }

        if (_input != null)
        {
            _input.PickupPressed -= TryPickupFromLook;
            _input.ShootPressed -= Shoot;
            _input.ReloadPressed -= Reload;
            _input.DropPressed -= Drop;
        }
    }

    //---Inventory callbacks---
    private void HandleGunEquipped(Gun gun)
    {
        _currentGun = gun;
        _aimSystem.SetGun(gun);
    }

    private void HandleGunUnequipped(Gun gun)
    {
        if (_currentGun == gun)
            _currentGun = null;

        _aimSystem.ClearGun(gun);
    }

    //---Input actions---
    private void Shoot()
    {
        if (_currentGun == null) return;
        _currentGun.Shoot();
    }

    private void Reload()
    {
        if (_currentGun == null) return;
        _currentGun.Reload(this);
    }

    private void Drop()
    {
        _inventory.Drop();
    }

    private void TryPickupFromLook()
    {
        if (!TryRaycastPickup(out DroppedWeapon dropped))
            return;

        _inventory.Pickup(dropped);
    }

    //---Helpers---
    private bool TryRaycastPickup(out DroppedWeapon dropped)
    {
        dropped = null;

        if (Camera.main == null)
            return false;

        Ray ray = Camera.main.ScreenPointToRay(
            new Vector2(Screen.width / 2f, Screen.height / 2f)
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, pickupCollisionLayerMask))
            return false;

        return hit.collider.GetComponentInParent<DroppedWeapon>() != null
            && (dropped = hit.collider.GetComponentInParent<DroppedWeapon>()) != null;
    }
}