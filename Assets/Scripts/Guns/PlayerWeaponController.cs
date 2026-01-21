using UnityEngine;
using UnityEngine.InputSystem;

//PlayerWeaponController тепер:
//читає input (polling)
//передає intent у WeaponInventory / Gun
//поєднує AimSystem і активний ган
namespace Guns
{
    public class PlayerWeaponController : MonoBehaviour 
    {
        [Header("Pickup")]
        [SerializeField] private LayerMask pickupCollisionLayerMask;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference shootAction;
        [SerializeField] private InputActionReference reloadAction;
        [SerializeField] private InputActionReference pickupAction;
        [SerializeField] private InputActionReference dropAction;
        [SerializeField] private InputActionReference swapAction;
    
        private WeaponInventory _inventory;
        private AimSystem _aimSystem;

        private Gun _currentGun; //щоб не питати WeaponInventory кожен раз та синхронізувати з AimSystem

        private void Awake()
        {
            _inventory = GetComponent<WeaponInventory>();
            _aimSystem = GetComponent<AimSystem>();
        }

        private void OnEnable()
        {
            _inventory.OnGunEquipped += HandleGunEquipped;
            _inventory.OnGunUnequipped += HandleGunUnequipped;
            _inventory.OnDropRequested += HandleDropRequested;
        }

        private void OnDisable()
        {
            _inventory.OnGunEquipped -= HandleGunEquipped;
            _inventory.OnGunUnequipped -= HandleGunUnequipped;
            _inventory.OnDropRequested -= HandleDropRequested;
        }

        private void Update()
        {
            if (shootAction.action.IsPressed()) _currentGun?.Shoot(_aimSystem.CurrentAim);

            if (reloadAction.action.WasPressedThisFrame()) _currentGun?.ReloadInstant();

            //if (swapAction.action.WasPressedThisFrame()) _inventory.Swap();

            if (pickupAction.action.WasPressedThisFrame()) TryPickup();
        
            if (dropAction.action.WasPressedThisFrame()) HandleDropRequested();
            
        }

        //AimSystem - Gun linking
        private void HandleGunEquipped(Gun gun)
        {
            _currentGun = gun;
            _aimSystem.SetAimProvider(gun);
        }

        private void HandleGunUnequipped(Gun gun)
        {
            if (_currentGun == gun)
                _currentGun = null;

            _aimSystem.ClearAimProvider(gun);
        }

        //PICKUP / DROP
        private void TryDrop()
        {
            if (_inventory.CurrentGun == null)
                return;

            Vector3 velocity = Vector3.zero;

            if (TryGetComponent<Rigidbody>(out var rb))
                velocity = rb.linearVelocity;

            _inventory.Drop(velocity);
        }
        private void HandleDropRequested()
        {
            TryDrop();
        }
        
        private void TryPickup()
        {
            if (!TryRaycastPickup(out DroppedWeapon dropped))
                return;

            _inventory.Pickup(dropped);
        }
    
        private bool TryRaycastPickup(out DroppedWeapon dropped)
        {
            dropped = null;
            if (Camera.main == null) return false;

            Ray ray = Camera.main.ScreenPointToRay(
                new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)
            );

            if (!Physics.Raycast(ray, out RaycastHit hit, 999f, pickupCollisionLayerMask))
                return false;

            return hit.collider.GetComponentInParent<DroppedWeapon>() is { } dw
                && (dropped = dw) != null;
        }
    }
}

