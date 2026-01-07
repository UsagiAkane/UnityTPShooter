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
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float maxSpeed = 100f;
        [SerializeField] private float jumpForce = 5f;
        
        private Rigidbody _rb;
        private Vector2 _moveInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
        }

        private void Update()
        {
            HandleInputs();
            HandleJump();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleInputs()
        {
            _moveInput = moveAction.action.ReadValue<Vector2>();
        }

        private void HandleMovement()
        {
            if (_moveInput == Vector2.zero) return;
            //movePositionMove();
            addForceMove();
        }

        private void movePositionMove()
        {
            _rb.MovePosition(_rb.position
                             + new Vector3(_moveInput.x, 0f, _moveInput.y)
                             * moveSpeed
                             * Time.fixedDeltaTime);
        }

        private void addForceMove()
        {
            Vector3 inputDir =
                new Vector3(_moveInput.x, 0f, _moveInput.y);

            // if (_rb.linearVelocity.magnitude < maxSpeed-5f)
            // {
            //     moveSpeed*=2f;
            // }
            
            Vector3 desiredAcceleration = inputDir * moveSpeed;
            _rb.AddForce(desiredAcceleration, ForceMode.Acceleration);

            Vector3 velocity = _rb.linearVelocity;
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);
            
            
            if (horizontal.magnitude > maxSpeed)
            {
                Vector3 limited = horizontal.normalized * maxSpeed;
                _rb.linearVelocity = new Vector3(limited.x, velocity.y, limited.z);
            }
        }
        
        private void HandleJump()
        {
            if (!jumpAction.action.triggered) return;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}