using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private WeaponInventory _inventory;
    private Gun _currentGun;

    public void Bind(WeaponInventory inventory)
    {
        _inventory = inventory;

        _inventory.OnGunEquipped += HandleGunEquipped;
        _inventory.OnGunUnequipped += HandleGunUnequipped;

        if (_inventory.CurrentGun != null) //Do you need playerWeaponController event if I dont use it?
            HandleGunEquipped(_inventory.CurrentGun);
        else
            ammoText.text = "--/--";
    }

    private void OnDestroy()
    {
        if (_inventory == null) return;

        _inventory.OnGunEquipped -= HandleGunEquipped;
        _inventory.OnGunUnequipped -= HandleGunUnequipped;

        UnsubscribeFromGun();
    }

    private void HandleGunEquipped(Gun gun)
    {
        UnsubscribeFromGun();

        _currentGun = gun;
        _currentGun.OnAmmoAmountChanged += UpdateAmmoText;
        //_currentGun.OnReloadStateChanged += HandleReloadState;

        UpdateAmmoText(_currentGun.CurrentAmmo, _currentGun.MaxAmmo);
    }

    private void HandleGunUnequipped(Gun gun)
    {
        if (gun != _currentGun) return;

        UnsubscribeFromGun();
        ammoText.text = "--/--";
    }

    private void UpdateAmmoText(int current, int max)
    {
        ammoText.text = $"{current}/{max}";
    }

    private void HandleReloadState(bool isReloading)
    {
        if (isReloading)
            ammoText.text = "RELOADING...";
    }

    private void UnsubscribeFromGun()
    {
        if (_currentGun == null) return;

        _currentGun.OnAmmoAmountChanged -= UpdateAmmoText;
        //_currentGun.OnReloadStateChanged -= HandleReloadState;
        _currentGun = null;
    }
}