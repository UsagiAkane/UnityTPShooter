using UnityEngine;

namespace Guns
{
    public class DroppedWeapon : MonoBehaviour
    {
        [SerializeField] private GunConfig config;
        [SerializeField] private int currentAmmo;

        public GunConfig GetConfig => config;
        public int GetCurrentAmmo => currentAmmo;

        public void Initialize(GunConfig cfg, int ammo)
        {
            config = cfg;
            currentAmmo = ammo;
        }
    }
}