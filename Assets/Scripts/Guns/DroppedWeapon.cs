using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField] private GunConfig config;

    private WeaponRuntimeData runtime;

    public void Initialize(GunConfig cfg, WeaponRuntimeData runtimeData)
    {
        config = cfg;
        runtime = runtimeData;
    }

    public GunConfig GetConfig() => config;
    public WeaponRuntimeData GetRuntime() => runtime;
}