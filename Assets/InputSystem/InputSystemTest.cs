using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputSystemTest : MonoBehaviour
    {
        [SerializeField] private InputActionReference jumpAction;
        
    

        private void Awake()
        {
            // bool canPlace = jumpAction.action.IsPressed();
            // Debug.Log("Can place = " + canPlace);
        }

        private void Update()
        {
            if (jumpAction.action.triggered)
            {
                Debug.Log("Jump");
                Jump();
            }
        }

        public void Jump()
        {
            
        }
    }
}