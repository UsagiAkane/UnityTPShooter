using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputSystemTest : MonoBehaviour
    {
        private Rigidbody capsuleRigidBody;

        private void Awake()
        {
            capsuleRigidBody = GetComponent<Rigidbody>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            Debug.Log("Jump " + context.phase);
            capsuleRigidBody.AddForce(Vector3.up*5f, ForceMode.Impulse);
        }
    }
}