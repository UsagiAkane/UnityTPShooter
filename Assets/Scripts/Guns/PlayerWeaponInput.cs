using System;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO FULL REWORK, DELETE ALL
public class PlayerWeaponInput : MonoBehaviour, IWeaponInputSource //TODO REWORK abstract class PlayerInput with all inputs.  PlayerWeaponInput inheritance PlayerInput
{
    public event Action Shoot;
    public event Action Reload;
    public event Action Pickup;
    public event Action Drop;
    public event Action Swap;

    [Header("Actions")]
    [SerializeField] private InputActionReference shoot;
    [SerializeField] private InputActionReference reload;
    [SerializeField] private InputActionReference pickup;
    [SerializeField] private InputActionReference drop;
    [SerializeField] private InputActionReference swap;

    private void OnEnable()
    {
        shoot.action.performed += OnShoot;
        reload.action.performed += OnReload;
        pickup.action.performed += OnPickup;
        drop.action.performed += OnDrop;
        swap.action.performed += OnSwap;

        // shoot.action.Enable();
        // reload.action.Enable();
        // pickup.action.Enable();
        // drop.action.Enable();
        // swap.action.Enable();
    }

    private void OnDisable()
    {
        // //не знаю чи треба?
        // shoot.action.Disable();
        // reload.action.Disable();
        
        shoot.action.performed -= OnShoot;
        reload.action.performed -= OnReload;
        pickup.action.performed -= OnPickup;
        drop.action.performed -= OnDrop;
        swap.action.performed -= OnSwap;
    }

    private void OnShoot(InputAction.CallbackContext _) => Shoot?.Invoke();
    private void OnReload(InputAction.CallbackContext _) => Reload?.Invoke();
    private void OnPickup(InputAction.CallbackContext _) => Pickup?.Invoke();
    private void OnDrop(InputAction.CallbackContext _) => Drop?.Invoke();
    private void OnSwap(InputAction.CallbackContext _) => Swap?.Invoke();
}