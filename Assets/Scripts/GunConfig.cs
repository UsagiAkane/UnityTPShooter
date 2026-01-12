using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "Weapons/GunConfig")]
public class GunConfig : ScriptableObject
{
    public GameObject droppepPF;
    public GameObject equipedPF;
    public string weaponName; //????
    public int clipSize;
    public float reloadTime;
    public int damage;
    public float fireRate;
    public float projectileSpeed = 20f;

    // Optional: spread, range, recoil, etc.
    public bool usesProjectile = true;
}