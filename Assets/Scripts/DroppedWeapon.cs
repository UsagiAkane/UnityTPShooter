using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField] private GunConfig config;

    public GunConfig GetConfig() => config;
}