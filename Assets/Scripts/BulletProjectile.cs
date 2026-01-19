using System;
using System.Collections;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rbBullet;

    private float _damage = 0f;
    private Vector3 _direction;
    private float _speed;
    private ObjectPool _pool;

    //Called by pool to spawn bullet
    public void Init(Vector3 direction, float speed, float dmg, float _lifeTime,ObjectPool pool)
    {
        _direction = direction;
        _speed = speed;
        _damage = dmg;
        _pool = pool;
        gameObject.SetActive(true);
        rbBullet.AddForce(_direction * _speed, ForceMode.Impulse);
        
        StartCoroutine(WaitForReturn(_lifeTime));
    }
    private IEnumerator WaitForReturn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ReturnToPool();
    }

    private void Update()
    {
        // _lifeTimer += Time.deltaTime;
        // if (_lifeTimer >= _lifeTime)
        // {
        //     ReturnToPool();
        // }
    }

    private void Awake()
    {
        //Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BulletTarget bulletTarget))
        {
            //Debug.Log("Target hit");
            bulletTarget.TookDamage(_damage);
        }
        //else Debug.Log("groud hit");

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
            rbBullet.linearVelocity = Vector3.zero;
            _pool.ReturnBulletProjectile(gameObject);
        }
    }
}