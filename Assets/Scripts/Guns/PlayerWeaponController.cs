using Guns.State_machine;
using Player.AimSystem;
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
        private Gun _currentGun; //TODO Do i really need this?
        

        private void Awake()
        {
            _inventory = GetComponent<WeaponInventory>();
            _aimSystem = GetComponent<AimSystem>();
        }

        private void OnEnable()
        {
            _aimSystem.OnAimComputed += HandleAimUpdated;
            
            _inventory.OnGunEquipped += HandleGunEquipped;
            _inventory.OnGunUnequipped += HandleGunUnequipped;
            _inventory.OnDropRequested += HandleDropRequested;
        }

        private void OnDisable()
        {
            _aimSystem.OnAimComputed -= HandleAimUpdated;
            
            _inventory.OnGunEquipped -= HandleGunEquipped;
            _inventory.OnGunUnequipped -= HandleGunUnequipped;
            _inventory.OnDropRequested -= HandleDropRequested;
        }

        private void Update()
        {
            if (shootAction.action.WasPressedThisFrame())
                _currentGun?.HandleFirePressed(_aimSystem.CurrentAim);

            if (shootAction.action.WasReleasedThisFrame())
                _currentGun?.HandleFireReleased();

            if (reloadAction.action.WasPressedThisFrame())
                _currentGun?.HandleReload();

            if (pickupAction.action.WasPressedThisFrame())
                TryPickup();

            if (dropAction.action.WasPressedThisFrame())
                HandleDropRequested();

            _currentGun?.Tick(Time.deltaTime);
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
            {
                _currentGun = null;
            }

            _aimSystem.ClearAimProvider(gun);
        }
        
        private void HandleAimUpdated(AimResult aim)
        {
            _currentGun?.OnAimUpdated(aim);
        }
        
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
            {
                Debug.unityLogger.Log("PlayerWeaponController TryPickup() TryRaycastPickup(out DroppedWeapon dropped) == null");
                return;
            }

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

