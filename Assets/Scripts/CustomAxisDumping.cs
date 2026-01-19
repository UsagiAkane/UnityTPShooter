using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomAxisDamping : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float xAxisDrag = 4f;
    [SerializeField] private float zAxisDrag = 4f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 velocity = _rb.linearVelocity;

        velocity.x *= (1f - xAxisDrag * Time.fixedDeltaTime);
        velocity.z *= (1f - zAxisDrag * Time.fixedDeltaTime);

        _rb.linearVelocity = velocity;
    }
}