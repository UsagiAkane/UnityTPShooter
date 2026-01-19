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

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheckSphereCenter;
        [SerializeField] private float groundRadius;
        //[SerializeField] private float maxFallingSpeed = -1f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private bool isGrounded;

        [Header("Aim/Camera Pitch")] 
        [SerializeField] private Transform cameraPitchTarget;
        [SerializeField] private float pitchSensitivity = 0.1f;
        [SerializeField] private float mouseSensitivity = 0.1f;
        //[SerializeField] private float rotationSpeed = 99999f;
        [SerializeField] private float pitchMin = -20f;
        [SerializeField] private float pitchMax = 60f;
        [SerializeField] private LayerMask aimCollisionLayerMask;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;
        //[SerializeField] private bool lookMoveDirection = true;


        private Vector2 lookDelta = Vector2.zero;
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
            HandleIsGrounded(); //for update mid air. Is this correct? Or call in HandleJump action?
                
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

        private void HandleInputs()
        {
            lookDelta = lookAction.action.ReadValue<Vector2>();

            if (moveAction?.action != null) _moveInput = moveAction.action.ReadValue<Vector2>();
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

        private void RotateFromLookAction()
        {
            float deltaYaw = lookDelta.x * mouseSensitivity * 0.1f;
            _yaw += deltaYaw;
            Quaternion targetRotation = Quaternion.Euler(0f, _yaw, 0f);
            _rb.MoveRotation(targetRotation);
        }

        private void RotateCameraPitchFromLook()
        {
            float deltaPitch = lookDelta.y * pitchSensitivity * 0.1f;
            _pitch -= deltaPitch;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            Vector3 euler = cameraPitchTarget.localEulerAngles;
            euler.x = _pitch;
            cameraPitchTarget.localEulerAngles = euler;
        }

        private void HandleJump()
        {
            if (jumpAction.action.triggered && isGrounded) // && HandleIsGrounded() because doesnt update mid air
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private bool HandleIsGrounded()
        {
            //----DO NOT WORK BECAUSE OF rb.velocity value on slopes----
            // Debug.Log(_rb.linearVelocity.y);
            // if(_rb.linearVelocity.y <= maxFallingSpeed)
            // {
            //     isGrounded = false;
            //     return false; // cancel jump mid air
            // }
            isGrounded = Physics.CheckSphere(groundCheckSphereCenter.position, groundRadius, aimCollisionLayerMask);
            return isGrounded;
        }
    }
}