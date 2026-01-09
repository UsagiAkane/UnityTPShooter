using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rbBullet;
    // [SerializeField] private Transform vfxHitGreen;
    // [SerializeField] private Transform vfxHitRed;
    
    void Start()
    {
        rbBullet.linearVelocity = transform.forward * 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            Debug.Log("Target hit");
        }
        else
        {
            Debug.Log("No target hit");
        }
        Destroy(gameObject);
    }
}
