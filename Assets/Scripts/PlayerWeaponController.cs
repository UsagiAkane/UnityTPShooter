using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerWeaponController : MonoBehaviour
{
    public static event Action<Gun> OnGunPickUp;
    public static event Action<Gun> OnGunDropDown;

    [Header("System")] [SerializeField] private Transform weaponHolder; // where to attach weapon
    [SerializeField] private LayerMask aimCollisionLayerMask;
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    [Header("Actions")] [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference pickUpAction;
    [SerializeField] private InputActionReference weaponDropAction;

    private Gun _currentGun;
    private GunConfig _currentGunConfig;
    private Vector3 _mouseWorldPosition = Vector3.zero;

    private void Update()
    {
        if (shootAction.action.IsPressed())
        {
            //Debug.Log("shooting");
            Shoot();
        }

        if (pickUpAction.action.triggered)
        {
            Debug.Log("Picking up");
            TryEquipFromLook();
        }

        if (weaponDropAction.action.triggered)
        {
            Debug.Log("Picking up");
            TryDropWeaponForward();
        }

        _mouseWorldPosition = HandleAimPosition();
        AlignGun();
    }

    private void Shoot()
    {
        if (_currentGun == null) return;
        _currentGun.Shoot();
    }

    private Vector3 HandleAimPosition() //dublicated from ThirdPersonController???
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimCollisionLayerMask))
        {
            return raycastHit.point;
        }
        else return Vector3.zero;
    }

    private void AlignGun()
    {
        if (_currentGun == null) return;
        weaponHolder.LookAt(_mouseWorldPosition);
    }

    private void TryEquipFromLook()
    {
        if (_currentGun != null) TryDropWeaponForward();

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, pickupCollisionLayerMask))
        {
            //Debug.Log("Picking up " + raycastHit.collider.name);
            if (raycastHit.collider.TryGetComponent<DroppedWeapon>(out var weapon))
            {
                _currentGunConfig = weapon.GetConfig();
                Destroy(weapon.gameObject);
                _currentGun = Instantiate(_currentGunConfig.equipedPF).GetComponent<Gun>();
                _currentGun.transform.SetParent(weaponHolder);
                _currentGun.transform.localPosition = Vector3.zero;
                _currentGun.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                _currentGun.transform.localScale = Vector3.one;
                OnGunPickUp?.Invoke(_currentGun);
                _currentGun.Initialize(_currentGunConfig);
                //Debug.Log("Picking up " + _currentGun);
            }
        }
    }

    private void TryDropWeaponForward(float dropImpulse = 2f, float torqueRange = 1.0f)
    {
        if (_currentGun == null)
        {
            Debug.Log("No weapon equipped to drop.");
            return;
        }

        Vector3 spawnPos = transform.position + transform.forward * 0.5f + transform.right * 0.5f; // 1 meter ahead
        Quaternion spawnRot = _currentGun.transform.rotation;

        Rigidbody rb = Instantiate(_currentGunConfig.droppepPF, spawnPos, spawnRot).GetComponent<Rigidbody>();
        rb.linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        rb.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        rb.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange * 0.05f, torqueRange * 0.05f)
        );
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        OnGunDropDown?.Invoke(_currentGun);
        Destroy(_currentGun.gameObject);
        _currentGun = null;
        _currentGunConfig = null;
    }
}