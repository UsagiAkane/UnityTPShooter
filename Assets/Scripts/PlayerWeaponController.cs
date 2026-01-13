using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // where to attach weapon
    [SerializeField] private LayerMask aimCollisionLayerMask;
    [SerializeField] private LayerMask pickupCollisionLayerMask;

    private Gun _currentGun;
    private GunConfig _currentGunConfig;

    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference pickUpAction;
    [SerializeField] private InputActionReference weaponDropAction;

    private Vector3 _mouseWorldPosition = Vector3.zero;

    private void Update()
    {
        if (shootAction.action.triggered)
        {
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

    public void AlignGunPitchToCrosshairFromWorldPoint(float maxPitchDeg = 60f)
    {
        if (_currentGun == null) return;

        Vector3 targetPoint = HandleAimPosition();
        if (targetPoint == Vector3.zero) return;

        // Compute direction to target
        Vector3 gunPos = _currentGun.transform.position;
        Vector3 dir = targetPoint - gunPos;

        // Keep yaw controlled by the parent/player; compute pitch relative to gun's local Up
        Vector3 localUp = _currentGun.transform.up;
        Vector3 dirProjected = Vector3.ProjectOnPlane(dir, localUp);
        if (dirProjected.sqrMagnitude < 0.0001f) return;

        Vector3 localForward = _currentGun.transform.forward;
        float pitch = Vector3.SignedAngle(Vector3.ProjectOnPlane(localForward, localUp), dirProjected, localUp);
        pitch = Mathf.Clamp(pitch, -maxPitchDeg, maxPitchDeg);

        // Apply only pitch, preserve yaw/roll
        Vector3 euler = _currentGun.transform.localEulerAngles;
        // Normalize angle wrap-around
        if (euler.x > 180f) euler.x -= 360f;
        euler.x = pitch;
        _currentGun.transform.localEulerAngles = new Vector3(euler.x, euler.y, 0f);
    }


    private void TryEquipFromLook()
    {
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
                //Debug.Log("Picking up " + _currentGun);
                _currentGun.transform.SetParent(weaponHolder);
                _currentGun.transform.localPosition = Vector3.zero;
                _currentGun.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                _currentGun.transform.localScale = Vector3.one;
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

        var droppedObj = Instantiate(_currentGunConfig.droppepPF, spawnPos, spawnRot);
        var rb = droppedObj.GetComponent<Rigidbody>();
        rb.linearVelocity = this.GetComponent<Rigidbody>().linearVelocity;
        rb.AddForce(transform.up * dropImpulse, ForceMode.Impulse);
        rb.AddForce(transform.forward * dropImpulse, ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange, torqueRange),
            Random.Range(-torqueRange * 0.05f, torqueRange * 0.05f)
        );
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(_currentGun.gameObject);
        _currentGun = null;
        _currentGunConfig = null;
    }
}