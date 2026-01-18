using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    private Gun _currentGun;

    private void Awake()
    {
        PlayerWeaponController.OnGunPickUp += HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown += HandleGunDropped;
    }
    
    private void UpdateAmmoText(int current, int max)
    {
        ammoText.text = $"{current}/{max}";
    }
    
    private void HandleGunPickedUp(Gun gun)
    {
        UnsubscribeFromGun();

        _currentGun = gun;
        _currentGun.OnAmmoAmountChanged += UpdateAmmoText;
    }

    private void HandleGunDropped(Gun gun)
    {
        if (gun != _currentGun) return;

        UnsubscribeFromGun();
        ammoText.text = "--/--";
    }

    private void UnsubscribeFromGun()
    {
        if (_currentGun == null) return;

        _currentGun.OnAmmoAmountChanged -= UpdateAmmoText;
        _currentGun = null;
    }
    
    private void OnDestroy()
    {
        PlayerWeaponController.OnGunPickUp -= HandleGunPickedUp;
        PlayerWeaponController.OnGunDropDown -= HandleGunDropped;

        UnsubscribeFromGun();
    }
}