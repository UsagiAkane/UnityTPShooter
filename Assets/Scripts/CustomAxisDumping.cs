using UnityEngine;

public class CustomAxisDamping : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float xAxisDrag = 4f;
    [SerializeField] private float zAxisDrag = 4f;

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.x *= (1f - xAxisDrag * Time.fixedDeltaTime);
        velocity.z *= (1f - zAxisDrag * Time.fixedDeltaTime);

        rb.linearVelocity = velocity;
    }
}