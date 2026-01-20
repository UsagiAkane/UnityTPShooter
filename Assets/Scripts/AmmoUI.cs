using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private Gun _currentGun;

    private void OnEnable()
    {
        PlayerWeaponController.OnGunPickUp += HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown += HandleGunDropped;
    }

    private void OnDisable()
    {
        PlayerWeaponController.OnGunPickUp -= HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown -= HandleGunDropped;

        UnsubscribeFromGun();
    }

    private void HandleGunPickedUp(Gun gun)
    {
        UnsubscribeFromGun();

        _currentGun = gun;
        _currentGun.OnAmmoAmountChanged += UpdateAmmoText;
        _currentGun.OnReloadStateChanged += HandleReloadState;

        UpdateAmmoText(
            _currentGun.AmmoCurrent,
            _currentGun.AmmoMax
        );
    }

    private void HandleGunDropped(Gun gun)
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
        _currentGun.OnReloadStateChanged -= HandleReloadState;
        _currentGun = null;
    }
}