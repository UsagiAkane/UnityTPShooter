using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject _bulletProjectilePrefab;
    [SerializeField] private int amount = 10;
    private bool usesProjectiles;
    private readonly List<GameObject> _freeBullets = new();

    public bool UsesProjectiles
    {
        get => usesProjectiles;
        set => usesProjectiles = value;
    }

    public void InitializePool(GameObject bulletPf, bool useProjectiles)
    {
        _bulletProjectilePrefab = bulletPf;
        UsesProjectiles = useProjectiles;
        Prewarm();
    }

    private void Prewarm()
    {
        for (int i = 0; i < amount; i++)
        {
            var b = Instantiate(_bulletProjectilePrefab);
            b.gameObject.SetActive(false);
            _freeBullets.Add(b);
        }
    }

    // Call from shooter
    public GameObject GetBulletProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject bullet;
        if (_freeBullets.Count > 0)
        {
            bullet = _freeBullets[0];
            _freeBullets.RemoveAt(0);
        }
        else
        {
            bullet = Instantiate(_bulletProjectilePrefab);
        }

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        //bullet.Init(direction, speed, this);
        return bullet;
    }

    //Called from bullet
    public void ReturnBulletProjectile(GameObject bullet)
    {
        bullet.SetActive(false);
        _freeBullets.Add(bullet);
    }

    private void OnDestroy()
    {
        Debug.Log("ObjectPool freeBullets =  " + _freeBullets.Count);
        for (int i = 0; i < _freeBullets.Count; i++)
        {
            Destroy(_freeBullets[i].gameObject);
            Debug.Log(i + ": Destroyed");
        }
    }
}