using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : MonoBehaviour
{
    // [SerializeField] private Transform vfxHitGreen;
    // [SerializeField] private Transform vfxHitRed;

    [SerializeField] private Rigidbody rbBullet;

    private Vector3 _direction;
    private float _speed;
    [SerializeField] float _lifeTime = 5f;
    private float _lifeTimer;
    private ObjectPool _pool;


    //Called by pool to spawn bullet
    public void Init(Vector3 direction, float speed, ObjectPool pool)
    {
        _direction = direction; //.normalized;
        _speed = speed;
        _lifeTimer = 0f;
        _pool = pool;
        gameObject.SetActive(true);
        rbBullet.AddForce(_direction * _speed, ForceMode.Impulse);
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= _lifeTime)
        {
            ReturnToPool();
        }
    }

    void Start()
    {
        //rbBullet.linearVelocity = transform.forward * 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BulletTarget bulletTarget))
        {
            Debug.Log("Target hit");
        }
        else
        {
            Debug.Log("No target hit");
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (_pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            _pool.ReturnBulletProjectile(this);
        }
    }
}