using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private BulletProjectile bulletProjectilePrefab;
    [SerializeField] private int amount = 20;

    private readonly List<BulletProjectile> _freeBullets = new();

    private void Awake()
    {
        Prewarm(amount);
    }
    
    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var b = Instantiate(bulletProjectilePrefab, transform);
            b.gameObject.SetActive(false);
            _freeBullets.Add(b);
        }
    }
    
    // Call from shooter
    public BulletProjectile GetBulletProjectile(Vector3 position, Quaternion rotation, Vector3 direction, float speed)
    {
        BulletProjectile bullet;
        if (_freeBullets.Count > 0)
        {
            bullet = _freeBullets[0];
            _freeBullets.RemoveAt(0);
        }
        else
        {
            bullet = Instantiate(bulletProjectilePrefab, transform);
        }

        bullet.Init(position, rotation, direction, speed, this);
        return bullet;
    }

    //Called from bullet
    public void ReturnBulletProjectile(BulletProjectile bullet)
    {
        bullet.gameObject.SetActive(false);
        _freeBullets.Add(bullet);
    }
}