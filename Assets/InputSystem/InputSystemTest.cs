using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputSystemTest : MonoBehaviour
    {
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private float baseSpeed = 10.0f;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
        }

        private void Update()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleMovement()
        {
           Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
           if (moveDirection == Vector2.zero) return;

           _rb.MovePosition(_rb.position + (new Vector3(moveDirection.x, 0f, moveDirection.y))*baseSpeed * Time.deltaTime);
        }
        
        private void HandleJump()
        {
            if (!jumpAction.action.triggered) return;
        }
    }
}