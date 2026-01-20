using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "Weapons/GunConfig")]
public class GunConfig : ScriptableObject
{
    public GameObject droppepPF;
    public GameObject equipedPF;
    public GameObject bulletPF;
    
    
    public string weaponName; //????
    public AudioClip shotSfx;
    
    public int damage;
    public float fireRate;
    public int clipSize;
    public float projectileSpeed = 20f;
    public float projectileLifeTimeSeconds = 5f;
    public float reloadTime;
    public AudioClip reloadSfx;

    // Optional: spread, range, recoil, etc.
    public bool usesProjectile = true;
}