using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // where to attach weapon
    [SerializeField] private LayerMask aimCollisionLayerMask;

    private Gun _currentGun;
    private GunConfig _currentGunConfig;

    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference pickUpAction;
    [SerializeField] private InputActionReference weaponDropAction;

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
    }

    private void Shoot()
    {
        if (_currentGun == null) return;
        _currentGun.Shoot();
    }

    private void TryEquipFromLook()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimCollisionLayerMask))
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
                _currentGun.transform.localRotation = Quaternion.Euler(180f, 0f, 90f);
                _currentGun.transform.localScale = Vector3.one;
            }
        }
    }

    public void TryDropWeaponForward(float dropImpulse = 2f, float torqueRange = 1.0f)
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