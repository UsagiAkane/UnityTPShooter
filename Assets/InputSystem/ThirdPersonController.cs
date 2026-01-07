using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Actions")] 
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference jumpAction;

        [Header("Params")] 
        [SerializeField] private float moveSpeed = 150f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float jumpForce = 5f;

        [Header("Pivo")] 
        //[SerializeField] private GameObject crosshair;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool lookMoveDirection = true;
        [SerializeField] private float mouseSens = 1f;
        private Rigidbody _rb;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleInputs();
            HandleCameraDirection();
            HandleJump();
            RotatePlayer();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleInputs()
        {
            _moveInput = moveAction.action.ReadValue<Vector2>();
        }

        private void HandleCameraDirection()
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            _moveDirection = forward * _moveInput.y + right * _moveInput.x;
        }
        
        private void HandleMovement()
        {
            if (_moveInput == Vector2.zero) return;
            AddForceMove();
            //movePositionMove();
        }


        private void AddForceMove()
        {
            //Vector3 inputDir =
                //new Vector3(_moveInput.x, 0f, _moveInput.y);

            //if (_rb.linearVelocity.magnitude < maxSpeed-5f) moveSpeed*=2f;

            Vector3 desiredAcceleration = _moveDirection * moveSpeed;
            _rb.AddForce(desiredAcceleration, ForceMode.Acceleration);

            Vector3 velocity = _rb.linearVelocity;
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);


            if (horizontal.magnitude > maxSpeed)
            {
                Vector3 limited = horizontal.normalized * maxSpeed;
                _rb.linearVelocity = new Vector3(limited.x, velocity.y, limited.z);
            }
        }
        private void MovePositionMove()=> _rb.MovePosition(_rb.position
                                                           + new Vector3(_moveInput.x, 0f, _moveInput.y)
                                                           * moveSpeed * Time.fixedDeltaTime);
        

        private void RotatePlayer()
        {
            // float x = _lookInput.x * rotationSpeed * Time.deltaTime;
            // float y = _lookInput.y * rotationSpeed * Time.deltaTime;
            if (lookMoveDirection && _moveDirection.sqrMagnitude > 0.001f)
            {
                //Quaternion delta = Quaternion.Euler(0f, x, 0f);
                
                Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            //_rb.MoveRotation(_rb.rotation * delta);
        }

        private void HandleJump()
        {
            if (!jumpAction.action.triggered) return;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}