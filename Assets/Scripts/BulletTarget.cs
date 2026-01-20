using System;
using UnityEngine;

public class BulletTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private AudioClip recieveDamageSound;//TODO REWORK?
    
    public static event Action<BulletTarget> OnTargetKilled;//TODO
    public static event Action<float> OnDamageTaken;

    [SerializeField] private float maxHealth = 100f;
    private float _health;

    private void Awake()
    {
        _health = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
        OnDamageTaken?.Invoke(damage);

        if (_health <= 0f)
        {
            OnTargetKilled?.Invoke(this);
            Destroy(gameObject);
        }
    }
}