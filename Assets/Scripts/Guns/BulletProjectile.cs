using System.Collections;
using UnityEngine;

namespace Guns
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rbBullet;
    
        private float _damage = 0f;
        private Vector3 _direction;
        private float _speed;
        private ObjectPool _pool;
        private IDamageInstigator _instigator;//to log who deal dmg


        //Called by pool to spawn bullet
        public void Init(Vector3 direction, float speed, float dmg, float _lifeTime, ObjectPool pool, IDamageInstigator ownerGameObject)
        {
            _direction = direction;
            _speed = speed;
            _damage = dmg;
            _pool = pool;
            gameObject.SetActive(true);
            rbBullet.AddForce(_direction * _speed, ForceMode.Impulse);
            _instigator = ownerGameObject;
        }

        private IEnumerator WaitForReturn(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageInfo info = new DamageInfo
                {
                    amount = _damage,
                    source = gameObject,
                    instigator = _instigator,//to log who deal dmg Nickname - gun - Who - headshot
                    hitPoint = transform.position,
                    hitDirection = transform.forward
                };

                damageable.TakeDamage(info);
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
                rbBullet.linearVelocity = Vector3.zero;
                //added for kekes
                rbBullet.angularVelocity = Vector3.zero;
                rbBullet.rotation = Quaternion.identity;
            
                _pool.ReturnBulletProjectile(gameObject);
            }
        }
    }
}