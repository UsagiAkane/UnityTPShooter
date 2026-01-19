using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "Weapons/GunConfig")]
public class GunConfig : ScriptableObject
{
    public GameObject droppepPF;
    public GameObject equipedPF;
    public GameObject bulletPF;
    
    public AudioClip shotSfx;
    
    public string weaponName; //????
    
    public int damage;
    public float fireRate;
    public int clipSize;
    public float projectileSpeed = 20f;
    public float projectileLifeTimeSeconds = 5f;
    public float reloadTime;

    // Optional: spread, range, recoil, etc.
    public bool usesProjectile = true;
}