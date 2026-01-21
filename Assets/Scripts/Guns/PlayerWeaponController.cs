using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    private WeaponInventory _inventory;
    private AimSystem _aimSystem;
    private IWeaponInputSource _input;

    private Gun _currentGun;

    private void Awake()
    {
        _inventory = GetComponent<WeaponInventory>();
        _aimSystem = GetComponent<AimSystem>();
        _input = GetComponent<IWeaponInputSource>();
    }

    private void OnEnable()
    {
        _inventory.OnGunEquipped += HandleGunEquipped;
        _inventory.OnGunUnequipped += HandleGunUnequipped;

        _input.Shoot += HandleShoot;//TODO call it in update?
        _input.Reload += HandleReload;//TODO call it in update?
        _input.Pickup += HandlePickup;//TODO call it in update?
        _input.Drop += HandleDrop;//TODO call it in update?
    }

    private void OnDisable()
    {
        _inventory.OnGunEquipped -= HandleGunEquipped;
        _inventory.OnGunUnequipped -= HandleGunUnequipped;

        _input.Shoot -= HandleShoot;
        _input.Reload -= HandleReload;
        _input.Pickup -= HandlePickup;
        _input.Drop -= HandleDrop;
    }

    //---Inventory---
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

    //---Input---
    private void HandleShoot()
    {
        if (_currentGun == null) return; //TODO AUTO RELOAD
        _currentGun.Shoot();
    }

    private void HandleReload()
    {
        if (_currentGun == null) return;
        _currentGun.ReloadInstant();//this);
    }

    private void HandleDrop()
    {
        _inventory.Drop();
    }

    private void HandlePickup()
    {
        if (!TryRaycastPickup(out DroppedWeapon dropped))
            return;

        _inventory.Pickup(dropped);
    }

    //--- Helpers ---
    private bool TryRaycastPickup(out DroppedWeapon dropped)
    {
        dropped = null;
        if (Camera.main == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(
            new Vector2(Screen.width / 2f, Screen.height / 2f)
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, pickupCollisionLayerMask))
            return false;

        return hit.collider.GetComponentInParent<DroppedWeapon>() is { } dw
            && (dropped = dw) != null;
    }
}
