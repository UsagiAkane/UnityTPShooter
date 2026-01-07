using System;
using System.Linq.Expressions;
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
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float mouseSensitivity = 0.1f;

        [Header("Camera")] 
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool lookMoveDirection = true;
        
        [Header("Aim/Camera Pitch")]
        [SerializeField] private Transform cameraPitchTarget;
        [SerializeField] private float pitchMin = -20f;
        [SerializeField] private float pitchMax = 60f;
        [SerializeField] private float pitchSensitivity = 1f;
        
        private float _pitch;

        private Rigidbody _rb;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;

        private float _yaw;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            //yaw from current facing
            _yaw = transform.eulerAngles.y;
        }

        private void Update()
        {
            HandleInputs();
            HandleCameraDirection();
            HandleJump();
            
            RotateFromLookAction();
            RotateCameraPitchFromLook();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
        
        private void RotateCameraPitchFromLook()
        {
            Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();
            float deltaPitch = lookDelta.y * pitchSensitivity;
            _pitch -= deltaPitch; // invert if you prefer: _pitch += deltaPitch
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            Vector3 euler = cameraPitchTarget.localEulerAngles;
            euler.x = _pitch;
            cameraPitchTarget.localEulerAngles = euler;
        }

        private void HandleInputs()
        {
            if (moveAction?.action != null)
                _moveInput = moveAction.action.ReadValue<Vector2>();
        }

        private void HandleCameraDirection()
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0; right.y = 0;
            forward.Normalize(); right.Normalize();

            _moveDirection = forward * _moveInput.y + right * _moveInput.x;
        }

        private void HandleMovement()
        {
            if (_moveInput == Vector2.zero) return;
            AddForceMove();
        }

        private void AddForceMove()
        {
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

        // Optional: if you want to keep MovePositionMove for some reason
        private void MovePositionMove() => _rb.MovePosition(_rb.position
                                                            + new Vector3(_moveInput.x, 0f, _moveInput.y)
                                                            * moveSpeed * Time.fixedDeltaTime);

        // Rotation driven by mouse delta (lookAction)
        private void RotateFromLookAction()
        {
            if (lookAction?.action == null) return;

            Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();
            //Debug.Log(lookDelta.x);
            float deltaYaw = lookDelta.x * mouseSensitivity *0.1f;

            _yaw += deltaYaw;
            // Apply yaw to the player (smooth)
            Quaternion targetRotation = Quaternion.Euler(0f, _yaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void HandleJump()
        {
            if (jumpAction?.action != null && jumpAction.action.triggered)
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // Optional: if you want to keep facing the movement direction when stationary
        // you can reintroduce a small RotatePlayer here and call it from Update.
    }
}