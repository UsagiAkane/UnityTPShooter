using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rbBullet;

    private float _damage = 0f;
    private Vector3 _direction;
    private float _speed;
    [SerializeField] float _lifeTime = 5f;
    private float _lifeTimer;
    private ObjectPool _pool;

    //Called by pool to spawn bullet
    public void Init(Vector3 direction, float speed, float dmg, ObjectPool pool)
    {
        _direction = direction;
        _speed = speed;
        _lifeTimer = 0f;
        _damage = dmg;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BulletTarget bulletTarget))
        {
            //Debug.Log("Target hit");
            bulletTarget.TookDamage(_damage);
        }
        else
        {
            Debug.Log("groud hit");
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
            _pool.ReturnBulletProjectile(gameObject);
        }
    }
}