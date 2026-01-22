using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputsManager : MonoBehaviour
    {
        private const string PLAYER_MAP_NAME = "Player";

        [SerializeField] private InputActionAsset inputActionAsset;
        private static PlayerInput _playerInput;

        private InputActionMap _playerMap;
    
        private void Awake()
        {
            InitializeMaps();
        }

        private void InitializeMaps()
        {
            //Debug.Log("1: Initializing Maps = " + inputActionAsset.FindActionMap(PLAYER_MAP_NAME));//---------
            
            _playerMap = inputActionAsset.FindActionMap(PLAYER_MAP_NAME);

            SetAllMapsActiveState(true);
        }
        
        private void SetAllMapsActiveState(bool isActive)
        {
            //Debug.Log("4: check AllMapsActiveState: " + isActive);//---------
            
            foreach (InputAction inputAction in inputActionAsset.actionMaps.SelectMany(inputActionMap =>
                         inputActionMap.actions))
            {
                SetActionActiveState(inputAction, isActive);
                //Debug.Log("4.1: SetActionActiveState = " + inputAction.name);
            }
        }

        private void SetActionMapActiveState(InputActionMap inputActionMap, bool isActive)
        {
            //Debug.Log("5: SetActionMapActiveState: " + isActive);//---------
            
            if (isActive) inputActionMap.Enable();
            else inputActionMap.Disable();
        }
        
        private void SetActionActiveState(InputAction action, bool isActive)
        {
            //Debug.Log("6: SetActionActiveState: " + isActive);//---------
            
            if(isActive) action.Enable();
            else action.Disable();
        }
    
        private void OnDestroy()
        {
            SetAllMapsActiveState(false);
        }
    }
}
