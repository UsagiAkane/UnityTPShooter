using UnityEngine;

using UnityEngine;

public class AimSystem : MonoBehaviour
{
    [SerializeField] private LayerMask aimCollisionLayerMask;
    [SerializeField] private Transform cameraTransform;

    private Gun _currentGun;
    private Vector3 _aimPoint;

    public Vector3 AimPoint => _aimPoint;

    public void SetGun(Gun gun)
    {
        _currentGun = gun;
    }

    public void ClearGun(Gun gun)
    {
        if (_currentGun == gun)
            _currentGun = null;
    }

    private void Update()
    {
        UpdateAimPoint();
        RotateGun();
    }

    private void UpdateAimPoint()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimCollisionLayerMask))
        {
            _aimPoint = hit.point;
        }
        else
        {
            _aimPoint = ray.origin + ray.direction * 100f;
        }
    }

    private void RotateGun()
    {
        if (_currentGun == null)
            return;

        // Debug.Log("ray");
         Debug.DrawRay(_currentGun.transform.position, _currentGun.transform.forward * 2f, Color.green);

        Vector3 dir = _aimPoint - _currentGun.transform.position;

        if (dir.sqrMagnitude < 0.001f)
            return;

        _currentGun.transform.rotation =
            Quaternion.LookRotation(dir.normalized, Vector3.up);
    }
}