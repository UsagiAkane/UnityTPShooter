using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField] private GunConfig config;

    public GunConfig GetConfig() => config;

//call from collision or input
    // public void Pickup(PlayerWeaponController player)
    // {
    //     //Pass config to player to equip, then destroy/detach
    //     //This requires a factory or a method on PlayerWeaponController to equip by config
    //     //For now we just destroy or disable in world
    //     Destroy(gameObject);
    // }
    
    
}