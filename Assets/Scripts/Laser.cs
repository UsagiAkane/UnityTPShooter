using UnityEngine;

public class Laser : Gun
{
    [SerializeField] private AudioClip shotSound;
    
    public override void Shoot()
    {
        if (!CanShoot) return;
        if (cooldown >= 0.01f) return;
        //Debug.Log("\ncurrent ammo = " + CurrentAmmo + "\nconfig name" + config.name);
        //Raycast logic

        CurrentAmmo--;

        cooldown = cooldown = 60f / config.fireRate;

        //play sfx
        SFXmanager.instance.PlaySFXClip(shotSound, transform, 1f);
    }
}