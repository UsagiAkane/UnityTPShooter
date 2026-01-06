using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputSystemTest : MonoBehaviour
    {
        private Rigidbody _capsuleRigidBody;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _capsuleRigidBody = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();

            _playerInput.onActionTriggered += PlayerInput_onActionTriggered;
        }

        private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
        {
            Debug.Log("context = " + context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Jump " + context.phase);
                _capsuleRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            }
        }
    }
}