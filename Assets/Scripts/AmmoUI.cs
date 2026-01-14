using System;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    private Gun currentGun;

    private void Awake()
    {
        PlayerWeaponController.OnGunPickUp += HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown += HandleGunDropped;
    }

    private void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }

    private void OnDestroy()
    {
        PlayerWeaponController.OnGunPickUp -= HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown -= HandleGunDropped;

        UnsubscribeFromGun();
    }



    private void HandleGunPickedUp(Gun gun)
    {
        UnsubscribeFromGun();

        currentGun = gun;
        currentGun.OnAmmoAmountChanged += UpdateAmmoText;

        
        //UpdateAmmoText(currentGun.CurrentAmmo,currentGun.);
    }

    private void HandleGunDropped(Gun gun)
    {
        if (gun != currentGun) return;

        UnsubscribeFromGun();
        ammoText.text = "--/--";
    }

    private void UnsubscribeFromGun()
    {
        if (currentGun == null) return;

        currentGun.OnAmmoAmountChanged -= UpdateAmmoText;
        currentGun = null;
    }

    private void UpdateAmmoText(int current, int max)
    {
        ammoText.text = $"{current}/{max}";
    }

}