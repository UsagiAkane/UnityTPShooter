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

        [Header("Camera")] 
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool lookMoveDirection = true;
        
        [Header("Aim/Camera Pitch")]
        [SerializeField] private Transform cameraPitchTarget;
        [SerializeField] private float pitchSensitivity = 0.1f;
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float rotationSpeed = 99999f;
        [SerializeField] private float pitchMin = -20f;
        [SerializeField] private float pitchMax = 60f;
        
        [SerializeField] private LayerMask aimCollisionLayerMask;
        [SerializeField] private Transform debugTransform;
        
        
        private Vector2 lookDelta = Vector2.zero;
        
        private Vector3 _mouseWorldPosition = Vector3.zero;
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
            lookDelta = lookAction.action.ReadValue<Vector2>();
            
            //-----------------
            _mouseWorldPosition = HandleAimPosition();
            debugTransform.position = _mouseWorldPosition;
            //ProjectileShootObsolete();
            //ProjectileShoot();
            
            
            //-----------------
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

        public Vector3 HandleAimPosition()
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            //Transform hitTransform = null; //hitscan check
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f,aimCollisionLayerMask))
            {
                //hitTransform = raycastHit.transform;//hitscan check
                return raycastHit.point;
            }
            else return Vector3.zero;
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
        
        private void MovePositionMove() => _rb.MovePosition(_rb.position
                                                            + new Vector3(_moveInput.x, 0f, _moveInput.y)
                                                            * moveSpeed * Time.fixedDeltaTime);
        
        private void RotateFromLookAction()
        {
            //Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();
            float deltaYaw = lookDelta.x * mouseSensitivity * 0.1f;
            _yaw += deltaYaw;
            Quaternion targetRotation = Quaternion.Euler(0f, _yaw, 0f);
            _rb.MoveRotation(targetRotation);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //Problem with sync to camera after collision
        }
        private void RotateCameraPitchFromLook()
        {
            //Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();
            float deltaPitch = lookDelta.y * pitchSensitivity * 0.1f;
            _pitch -= deltaPitch;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            Vector3 euler = cameraPitchTarget.localEulerAngles;
            euler.x = _pitch;
            cameraPitchTarget.localEulerAngles = euler;
        }
        
        private void HandleJump()
        {
            if (jumpAction.action.triggered) //TODO isGrounded check
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
       
    }
}