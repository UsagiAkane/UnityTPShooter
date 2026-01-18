using UnityEngine;

public class Laser : Gun
{
    [SerializeField] private AudioClip shotSound;

    public override void Shoot()
    {
        if (!CanShoot) return;
        if (cooldown >= 0.01f) return;

        if (Physics.Raycast(firePoint.position, transform.TransformDirection(Vector3.forward),
                out RaycastHit raycastHit, 100f))
        {
            Debug.DrawRay(firePoint.position, raycastHit.point, Color.red);
            if (raycastHit.collider.TryGetComponent(out BulletTarget bulletTarget))
            {
                //Debug.Log("Target hit");
                bulletTarget.TookDamage(50f);
            }
            else
            {
                Debug.Log("groud hit");
            }
        }

        CurrentAmmo--;
        cooldown = cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(shotSound, transform, 1f);
    }
}