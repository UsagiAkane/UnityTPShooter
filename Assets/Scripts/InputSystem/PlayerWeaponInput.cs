using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInput : MonoBehaviour
{
    public event Action ShootPressed;
    public event Action ReloadPressed;
    public event Action PickupPressed;
    public event Action DropPressed;

    [SerializeField] private InputActionReference shoot;
    [SerializeField] private InputActionReference reload;
    [SerializeField] private InputActionReference pickup;
    [SerializeField] private InputActionReference drop;

    private void Update()
    {
        if (shoot.action.IsPressed())
            ShootPressed?.Invoke();

        if (reload.action.triggered)
            ReloadPressed?.Invoke();

        if (pickup.action.triggered)
            PickupPressed?.Invoke();

        if (drop.action.triggered)
            DropPressed?.Invoke();
    }
}
